namespace OnlyCreateDatabase.Model
{
    public class FileUpload
    {
        public int Id {  get; set; }
        public string FileName {  get; set; }
        public string FileNameDatabase { get; set; }
        public string FilePath {  get; set; }
        public byte[] FileBlob { get; set; }
        public DateTime Uploaded { get; set; } = DateTime.Now;

        public FileUpload(string fileName, string fileNameDatabase, string filePath)
        {
            FileName = fileName;
            FileNameDatabase = fileNameDatabase;
            FilePath = filePath;
        }
    }
}
