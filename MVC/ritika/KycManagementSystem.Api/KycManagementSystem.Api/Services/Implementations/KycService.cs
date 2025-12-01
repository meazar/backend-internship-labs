using KycManagementSystem.Api.Models.DTOs.Kyc;
using KycManagementSystem.Api.Models.DTOs.Pagination;
using KycManagementSystem.Api.Repositories.Interfaces;
using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Services.Interfaces;

using Dapper;
namespace KycManagementSystem.Api.Services.Implementations
{
    public class KycService:IKycService
    {
        private readonly IKycRepository _kycRepo;
        private readonly ILogsRepository _logRepo;
        private readonly IKycHistoryService _history;


        public KycService(IKycRepository kycRepo, ILogsRepository logRepo, IKycHistoryService history)
        {
            _kycRepo = kycRepo;
            _logRepo = logRepo;
            _history = history;
        }
        public async Task<int> CreateKycAsync(CreateKycDto dto)
        {
            string? photoFileName = null;
            string? docFrontFileName = null;
            string? docBackFileName = null;

            var folderPath = Path.Combine("wwwroot", "kyc-documents");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            
            if (dto.Photo != null)
            {
                photoFileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Photo.FileName)}";
                var path = Path.Combine(folderPath, photoFileName);

                using var stream = new FileStream(path, FileMode.Create);
                await dto.Photo.CopyToAsync(stream);
            }

            
            var kyc = new KycProfileDto
            {
                UserId = dto.UserId,
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Address = dto.Address,
                FatherName = dto.FatherName,
                GrandfatherName = dto.GrandfatherName,
                Occupation = dto.Occupation,
                AnnualIncome = dto.AnnualIncome,
                Nationality = dto.Nationality,

                PhotoPath = photoFileName,
                
                Status = "Pending",
                Remarks = dto.Remarks
            };

            int kycId = await _kycRepo.CreateKycAsync(kyc);
            await _history.AddAsync( kycId, "KYC Created", dto.Remarks, dto.UserId);

            await _logRepo.LogAsync(new ApiLog
            {
                Path = "CreateKyc",
                Method = "POST",
                StatusCode = 200,
                RequestBody = System.Text.Json.JsonSerializer.Serialize(dto)
            });

            return kycId;
        }


        public async Task<KycProfileDto?> GetKycByIdAsync(int id)
        {
            var kyc = await _kycRepo.GetByIdAsync(id);
            if (kyc == null) return null;

            return new KycProfileDto
            {
                Id = kyc.Id,
                UserId = kyc.UserId,
                FullName = kyc.FullName,
                DateOfBirth = kyc.DateOfBirth,
                Gender = kyc.Gender,
                Address = kyc.Address,
                FatherName = kyc.FatherName,
                GrandfatherName = kyc.GrandfatherName,
                Occupation = kyc.Occupation,
                AnnualIncome = kyc.AnnualIncome,
                Nationality = kyc.Nationality,
                PhotoPath = kyc.PhotoPath,
                
                Status = kyc.Status,
                Remarks = kyc.Remarks,
                CreatedAt = kyc.CreatedAt,
                UpdatedAt = kyc.UpdatedAt
            };
        }

        public async Task<PageResult<KycProfileDto>> GetPagedKycAsync(PageRequest pageRequest)
        {
            var list = await _kycRepo.GetPagedAsync(pageRequest);
            var total = await _kycRepo.CountAsync();

            var dtoList = list.Select(kyc => new KycProfileDto
            {
                Id = kyc.Id,
                UserId = kyc.UserId,
                FullName = kyc.FullName,
                DateOfBirth = kyc.DateOfBirth,
                Gender = kyc.Gender,
                Address = kyc.Address,
                FatherName = kyc.FatherName,
                GrandfatherName = kyc.GrandfatherName,
                Occupation = kyc.Occupation,
                AnnualIncome = kyc.AnnualIncome,
                Nationality = kyc.Nationality,
                PhotoPath = kyc.PhotoPath,
                Status = kyc.Status,
                Remarks = kyc.Remarks,
                CreatedAt = kyc.CreatedAt,
                UpdatedAt = kyc.UpdatedAt
            });

            return new PageResult<KycProfileDto>
            {
                Items = dtoList,
                TotalCount = total,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize
            };
        }

        public async Task UpdateKycAsync(int id, CreateKycDto dto)
        {
            
            var existing = await _kycRepo.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("KYC not found");

            string? photoFileName = existing.PhotoPath;

            var folderPath = Path.Combine("wwwroot", "kyc-documents");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            if (dto.Photo != null)
            {
                photoFileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Photo.FileName)}";
                var path = Path.Combine(folderPath, photoFileName);

                using var stream = new FileStream(path, FileMode.Create);
                await dto.Photo.CopyToAsync(stream);
            }

            

            var kyc = new KycProfileDto
            {
                Id = id,
                UserId = dto.UserId,
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Address = dto.Address,
                FatherName = dto.FatherName,
                GrandfatherName = dto.GrandfatherName,
                Occupation = dto.Occupation,
                AnnualIncome = dto.AnnualIncome,
                Nationality = dto.Nationality,

                PhotoPath = photoFileName,
              
             
                Remarks = dto.Remarks,
                UpdatedAt = DateTime.Now
            };

            await _kycRepo.UpdateKycAsync(kyc);
            await _history.AddAsync(id, "KYC Updated", dto.Remarks, dto.UserId);

        }


        public async Task UpdateKycStatusAsync(int kycId, KycStatusUpdateDto dto)
        {
            await _kycRepo.UpdateStatusAsync(kycId, dto.Status,null, dto.Remarks);
            await _history.AddAsync(kycId, $"Status Changed to {dto.Status}", dto.Remarks, dto.OfficerId);
        }


    }
}
