namespace OnlyCreateDatabase.DTO
{
    public class ListDTO<T>
    {
        public int? count { get; set; }
        public T data { get; set; }

    }
}
