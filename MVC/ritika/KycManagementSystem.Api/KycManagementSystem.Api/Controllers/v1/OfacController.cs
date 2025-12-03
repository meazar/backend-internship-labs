using KycManagementSystem.Api.Models.DTOs.Ofac;
using KycManagementSystem.Api.Models.DTOs.Pagination;
using KycManagementSystem.Api.Services.Implementations;
using KycManagementSystem.Api.Services.Interfaces;
using KycManagementSystem.Api.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KycManagementSystem.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/ofac")]
    [Authorize(Roles = "Admin,Officer")]
    public class OfacController : ControllerBase
    {
        private readonly IOfacService _ofacService;
        private readonly IKycService _kycService;
        public OfacController(IOfacService ofacService, IKycService kycService)
        {
            _ofacService = ofacService;
            _kycService = kycService;
        }
        [Authorize(Roles = "Admin,Officer")]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var kycList = await _kycService.GetPagedKycAsync(new PageRequest { PageNumber = 1, PageSize = 500 });

            var kyc = kycList.Items.FirstOrDefault(k =>
                k.FullName.Equals(name, StringComparison.OrdinalIgnoreCase));

            string nationality = kyc?.Nationality ?? "Unknown";

            var result = await _ofacService.SearchAsync(name, nationality);
            return Ok(ApiResponse<OfacCheckResponse>.Ok(result));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var sanctions = await _ofacService.GetAllSanctionsAsync();
            return Ok(ApiResponse<object>.Ok(sanctions));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> AddSanction([FromBody] OfacMatchDto dto)
        {
            var id = await _ofacService.AddDummySanctionAsync(dto);
            return Ok(ApiResponse<object>.Ok(new { SanctionId = id }));
        }

    }
}
