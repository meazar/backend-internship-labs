using LoanManagementSystem.IRepository;
using LoanManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<DocumentController> _logger;

        public DocumentController(IDocumentRepository documentRepository, IWebHostEnvironment environment, ILogger<DocumentController> logger)
        {
            _documentRepository = documentRepository;
            _environment = environment;
            _logger = logger;
        }
        [HttpPost("upload")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UploadDocument([FromForm] UploadDocumentRequest request)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                {
                    return BadRequest(new { message = "No file uploaded" });
                }

                // Validate file type
                var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx" };
                var fileExtension = Path.GetExtension(request.File.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { message = "Invalid file type" });
                }

                // Validate file size (5MB max)
                if (request.File.Length > 5 * 1024 * 1024)
                {
                    return BadRequest(new { message = "File size exceeds 5MB limit" });
                }

                // Create uploads directory if it doesn't exist


                var webRoot = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath ?? Directory.GetCurrentDirectory(), "wwwroot");

                var uploadsPath = Path.Combine(webRoot, "uploads", "documents");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsPath, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(stream);
                }

                // Create document record
                var document = new Document
                {
                    ApplicationId = request.ApplicationId,
                    UserId = request.UserId,
                    DocumentType = request.DocumentType,
                    FileName = request.File.FileName,
                    FileUrl = $"/uploads/documents/{fileName}",
                    UploadedAt = DateTime.UtcNow
                };

                var documentId = await _documentRepository.UploadDocumentAsync(document);
                if (documentId > 0)
                {
                    return Ok(new
                    {
                        message = "Document uploaded successfully",
                        documentId,
                        fileUrl = document.FileUrl
                    });
                }

                return StatusCode(500, new { message = "Failed to save document record" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading document");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("application/{applicationId}")]
        [Authorize(Roles = "Admin,Officer")]
        public async Task<IActionResult> GetApplicationDocuments(int applicationId)
        {
            try
            {
                var documents = await _documentRepository.GetDocumentsByApplicationAsync(applicationId);
                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting application documents");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin,Officer")]

        public async Task<IActionResult> GetUserDocuments(int userId)
        {
            try
            {
                var documents = await _documentRepository.GetDocumentsByUserAsync(userId);
                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user documents");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpDelete("{documentId}")]
        [Authorize(Roles = "Admin,Officer")]
        public async Task<IActionResult> DeleteDocument(int documentId)
        {
            try
            {
                var document = await _documentRepository.GetDocumentByIdAsync(documentId);
                if (document == null)
                {
                    return NotFound(new { message = "Document not found" });
                }

                // Delete physical file
                var filePath = Path.Combine(_environment.WebRootPath, document.FileUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Delete database record
                var success = await _documentRepository.DeleteDocumentAsync(documentId);
                if (!success)
                {
                    return StatusCode(500, new { message = "Failed to delete document record" });
                }

                return Ok(new { message = "Document deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting document");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("download/{documentId}")]
        public async Task<IActionResult> DownloadDocument(int documentId)
        {
            try
            {
                var document = await _documentRepository.GetDocumentByIdAsync(documentId);
                if (document == null)
                {
                    return NotFound(new { message = "Document not found" });
                }

                var filePath = Path.Combine(_environment.WebRootPath, document.FileUrl.TrimStart('/'));
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { message = "File not found" });
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var contentType = GetContentType(document.FileName);

                return File(fileBytes, contentType, document.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading document");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream",
            };
        }
    }
}
