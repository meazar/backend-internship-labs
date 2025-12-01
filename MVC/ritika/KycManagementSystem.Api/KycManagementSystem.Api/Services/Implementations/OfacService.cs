using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Models.DTOs.Ofac;
using KycManagementSystem.Api.Repositories.Interfaces;
using KycManagementSystem.Api.Services.Interfaces;

public class OfacService : IOfacService
{
    private readonly IOfacRepository _ofacRepo;

    public OfacService(IOfacRepository ofacRepo)
    {
        _ofacRepo = ofacRepo;
    }

    public async Task<OfacCheckResponse> SearchAsync(string fullName, string? nationality = null)
    {
        var sanctions = await _ofacRepo.SearchSanctionsAsync(fullName, nationality);

        var matches = sanctions.Select(s => new OfacMatchDto
        {
            Id = s.Id,
            FullName = s.FullName,
            AliasName = s.AliasName,
            Nationality = s.Nationality,
            Category = s.Category,
            RiskLevel = s.RiskLevel,
            Notes = s.Notes
        }).ToList();

        return new OfacCheckResponse
        {
            FullName = fullName,
            Nationality = nationality ?? "Unknown",
            IsSanctioned = matches.Any(),
            CheckedOn = DateTime.Now,
            Matches = matches
        };
    }

    public async Task<IEnumerable<OfacMatchDto>> GetAllSanctionsAsync()
    {
        var sanctions = await _ofacRepo.GetAllSanctionsAsync();

        return sanctions.Select(s => new OfacMatchDto
        {
            Id = s.Id,
            FullName = s.FullName,
            AliasName = s.AliasName,
            Nationality = s.Nationality,
            Category = s.Category,
            RiskLevel = s.RiskLevel,
            Notes = s.Notes
        });
    }

    public async Task<int> AddDummySanctionAsync(OfacMatchDto dto)
    {
        var sanction = new DummyOfacRecord
        {
            FullName = dto.FullName,
            AliasName = dto.AliasName,
            Nationality = dto.Nationality,
            Category = dto.Category,
            RiskLevel = dto.RiskLevel,
            Notes = dto.Notes
        };

        return await _ofacRepo.AddDummySanctionAsync(sanction);
    }
}
