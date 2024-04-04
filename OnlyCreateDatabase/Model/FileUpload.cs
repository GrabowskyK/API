namespace OnlyCreateDatabase.Model
{
    public class FileUpload
    {
        public int Id {  get; set; }
        public string FileName { get; set; }
        public string FilePath {  get; set; }
        public DateTime Uploaded { get; set; } = DateTime.Now;
    }
}
