using OnlyCreateDatabase.Model;

namespace OnlyCreateDatabase.Services.FileUploadServ
{
    public interface IFileUploadService
    {
        Task<FileUpload> SaveFileAsync(IFormFile File);
        FileUpload? GetFile(int fileId);
        void DeleteFileAsync(int fileId);
        bool IsFileExist(int fileId);
    }
}