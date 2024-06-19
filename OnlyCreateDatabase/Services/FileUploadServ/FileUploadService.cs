using Microsoft.EntityFrameworkCore;
using OnlyCreateDatabase.Database;
using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.Services.FileUploadServ
{
    public class FileUploadService : IFileUploadService
    {
        private readonly DatabaseContext databaseContext;
        private readonly IConfiguration configuration;

        public FileUploadService(DatabaseContext _databaseContext, IConfiguration _configuration)
        {
            databaseContext = _databaseContext;
            configuration = _configuration;
        }

        public FileUpload? GetFile(int fileId) => databaseContext.Files
            .Where(f => f.Id == fileId)
            .SingleOrDefault();



        public async Task<FileUpload> SaveFileAsync(IFormFile File)
        {
            string extention = Path.GetExtension(File.FileName);

            string fileName = Guid.NewGuid().ToString() + extention;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "UploadFiles");
            string fullPath = Path.Combine(path, fileName);

            try
            {
                FileUpload fileUpload = new FileUpload(File.FileName, fileName, fullPath);
                using (var memoryStream = new MemoryStream())
                {
                    await File.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    fileUpload.FileBlob = fileBytes;
                }
                SaveFileInDatabase(fileUpload);
                using FileStream stream = new FileStream(fullPath, FileMode.Create);
                File.CopyTo(stream);
                return fileUpload;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void SaveFileInDatabase(FileUpload fileUpload)
        {
            databaseContext.Files.Add(fileUpload);
            databaseContext.SaveChanges();
        }

        public async void DeleteFileAsync(int fileId)
        {
            var file = databaseContext.Files.Where(f => f.Id == fileId).FirstOrDefault();
            databaseContext.Files.Remove(file);
            var exercise = await databaseContext.Exercise.Where(e => e.FileUploadId == fileId).FirstOrDefaultAsync();
            exercise.FileUploadId = null;
            exercise.FileUpload = null;
            databaseContext.Exercise.Update(exercise);

            databaseContext.SaveChanges();
        }

        public bool IsFileExist(int fileId) => databaseContext.Files.Any(f => f.Id == fileId);

    }
}
