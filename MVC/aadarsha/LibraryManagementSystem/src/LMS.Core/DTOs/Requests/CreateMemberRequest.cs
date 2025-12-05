using System.ComponentModel.DataAnnotations;

namespace LMS.Core.DTOs.Requests.Members
{
    public class CreateMemberRequest
    {
        [Required]
        public int UserId { get; set; }

        [MaxLength(20)]
        public string? MemberId { get; set; }

        [MaxLength(20)]
        public string MembershipType { get; set; } = "Regular";

        public int MaxBooksAllowed { get; set; } = 5;
    }
}
