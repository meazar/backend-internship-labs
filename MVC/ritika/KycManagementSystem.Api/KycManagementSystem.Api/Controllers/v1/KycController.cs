using KycManagementSystem.Api.Models.DTOs.Kyc;
using KycManagementSystem.Api.Models.DTOs.Pagination;
using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Services.Interfaces;
using KycManagementSystem.Api.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KycManagementSystem.Api.Controllers.v1
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/v1/kyc")]
    public class KycController : ControllerBase
    {
        private readonly IKycService _kycService;
        private readonly IOfacService _ofacService;

        public KycController(IKycService kycService, IOfacService ofacService)
        {
            _kycService = kycService;
            _ofacService = ofacService;
        }
        [Authorize(Roles = "Client")]
        [HttpPost("create-kyc")]
        public async Task<IActionResult> CreateKyc([FromForm] CreateKycDto dto)
        {
            var kycId = await _kycService.CreateKycAsync(dto);
            return Ok(ApiResponse<object>.Ok(new { KycId = kycId }));
        }

        [Authorize(Roles = "Client,Officer,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetKycById(int id)
        {
            var role = User.FindFirst("role")?.Value;
            var userId = int.Parse(User.FindFirst("id")!.Value);

            if (role == "Client")
            {
                var kycId = await _kycService.GetKycByIdAsync(id);
                if (kycId.UserId != userId)
                    return Unauthorized("You cannot access another user's KYC.");
            }

            var kyc = await _kycService.GetKycByIdAsync(id);

            if (kyc == null)
                return NotFound(ApiResponse<string>.Fail("KYC not found"));

            return Ok(ApiResponse<KycProfileDto>.Ok(kyc));
        }


        [Authorize(Roles = "Client,Officer,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKyc(int id, [FromForm] CreateKycDto dto)
        {
            await _kycService.UpdateKycAsync(id, dto);
            return Ok(ApiResponse<string>.Ok("Updated"));
        }

        [Authorize(Roles = "Admin,Officer")]
        [HttpPost("paged")]
        public async Task<IActionResult> GetPagedKyc([FromBody] PageRequest request)
        {
            var result = await _kycService.GetPagedKycAsync(request);
            return Ok(ApiResponse<PageResult<KycProfileDto>>.Ok(result));
        }

        [Authorize(Roles = "Admin,Officer")]
        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] KycStatusUpdateDto dto)
        {
            await _kycService.UpdateKycStatusAsync(id, dto);
            return Ok(ApiResponse<string>.Ok("Status updated"));
        }

        [Authorize(Roles = "Admin,Officer")]
        [HttpGet("ofac-check/{id}")]
        public async Task<IActionResult> CheckOfac(int id)
        {
            var kyc = await _kycService.GetKycByIdAsync(id);
            if (kyc == null)
                return NotFound(ApiResponse<string>.Fail("KYC not found"));

            var result = await _ofacService.SearchAsync(kyc.FullName, kyc.Nationality);

            return Ok(ApiResponse<object>.Ok(result));
        }

        [Authorize(Roles = "Admin,Officer")]
        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetKycHistory(int id, [FromServices] IKycHistoryService historyService)
        {
            var history = await historyService.GetByKycIdAsync(id);
            return Ok(ApiResponse<IEnumerable<KycHistory>>.Ok(history));
        }


    }
}
