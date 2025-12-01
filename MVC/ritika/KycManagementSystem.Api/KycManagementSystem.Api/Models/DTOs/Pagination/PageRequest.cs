namespace KycManagementSystem.Api.Models.DTOs.Pagination
{
    public class PageRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
