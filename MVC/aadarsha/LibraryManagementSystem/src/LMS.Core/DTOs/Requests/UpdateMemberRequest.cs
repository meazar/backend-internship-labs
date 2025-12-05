using System.ComponentModel.DataAnnotations;

namespace LMS.Core.DTOs.Requests;

public class UpdateMemberRequest
{
    [MaxLength(20)]
    public string? MemberId { get; set; }

    [MaxLength(20)]
    public string? MembershipType { get; set; }

    [Range(1, 20)]
    public int? MaxBooksAllowed { get; set; }

    public bool? IsMembershipActive { get; set; }
}
