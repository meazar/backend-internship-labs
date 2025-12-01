public class NewApiResponse<T>
{
    public string? code { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}