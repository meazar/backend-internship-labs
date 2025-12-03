using KycManagementSystem.Api.Models.DTOs.Documents;
using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Services.Implementations;
using KycManagementSystem.Api.Services.Interfaces;
using KycManagementSystem.Api.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KycManagementSystem.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/documents")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IWebHostEnvironment _env;
        private readonly IKycService _kycService;

        public DocumentsController(IDocumentService documentService, IWebHostEnvironment env, IKycService kycService)
        {
            _documentService = documentService;
            _env = env;
            _kycService = kycService;
        }
        [Authorize(Roles = "Client,Officer,Admin")]
        [HttpPost("{kycId}")]
        public async Task<IActionResult> UploadDocument(int kycId, [FromForm] DocumentUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file uploaded");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await dto.File.CopyToAsync(stream);

            var document = new KycDocument
            {
                KycId = kycId,
                DocType = dto.DocType,
                FilePath = filePath,       
                UploadedAt = DateTime.UtcNow
            };

            await _documentService.UploadDocumentAsync(dto);

            return Ok(ApiResponse<string>.Ok("File uploaded successfully"));
        }



        [Authorize(Roles = "Client,Officer,Admin")]
        [HttpGet("{kycId}")]
        public async Task<IActionResult> GetDocuments(int kycId)
        {
            var role = User.FindFirst("role")?.Value;

            if (role == "Client")
            {
                var userId = int.Parse(User.FindFirst("id")!.Value);
                var kyc = await _kycService.GetKycByIdAsync(kycId);

                if (kyc == null || kyc.UserId != userId)
                    return Unauthorized("You cannot access another user's KYC.");
            }

            var docs = await _documentService.GetDocumentsByKycIdAsync(kycId);

            return Ok(new
            {
                success = true,
                data = docs
            });
        }
        [Authorize(Roles = "Officer,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse<string>.Fail("File is required."));

            await _documentService.UpdateDocumentAsync(id, file);

            return Ok(ApiResponse<string>.Ok("Document updated successfully."));
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            await _documentService.DeleteDocumentAsync(id);
            return Ok(ApiResponse<string>.Ok("Document deleted successfully."));
        }


    }
}
