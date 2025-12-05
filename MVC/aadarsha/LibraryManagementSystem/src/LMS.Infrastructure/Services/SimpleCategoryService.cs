using LMS.Core.DTOs.Requests;
using LMS.Core.DTOs.Responses;
using LMS.Core.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMS.API.Services
{
    public class SimpleCategoryService
    {
        private readonly LMSDbContext _context;
        private readonly ILogger<SimpleCategoryService> _logger;

        public SimpleCategoryService(LMSDbContext context, ILogger<SimpleCategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<CategoryResponse>> CreateCategoryAsync(
            CreateCategoryRequest request
        )
        {
            try
            {
                var existingCategory = await _context.Categories.FirstOrDefaultAsync(c =>
                    c.Name == request.Name
                );

                if (existingCategory != null)
                {
                    return ApiResponse<CategoryResponse>.ErrorResponse(
                        "Category with this name already exists."
                    );
                }

                if (request.ParentCategoryId.HasValue)
                {
                    var parentCategory = await _context.Categories.FindAsync(
                        request.ParentCategoryId.Value
                    );

                    if (parentCategory == null)
                    {
                        return ApiResponse<CategoryResponse>.ErrorResponse(
                            "Parent category not found."
                        );
                    }
                }

                var category = new Category
                {
                    Name = request.Name,
                    Description = request.Description,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                var response = MapToResponse(category);
                return ApiResponse<CategoryResponse>.SuccessResponse(
                    response,
                    "Category created successfully."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                return ApiResponse<CategoryResponse>.ErrorResponse(
                    "An error occurred while creating category."
                );
            }
        }

        public async Task<ApiResponse<CategoryResponse>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _context
                    .Categories.Include(c => c.Name)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    return ApiResponse<CategoryResponse>.ErrorResponse("Category not found.");
                }

                var response = MapToResponse(category);
                return ApiResponse<CategoryResponse>.SuccessResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting category by ID");
                return ApiResponse<CategoryResponse>.ErrorResponse(
                    "An error occurred while retrieving category."
                );
            }
        }

        public async Task<ApiResponse<IEnumerable<CategoryResponse>>> GetAllCategoriesAsync(
            int pageNumber = 1,
            int pageSize = 10
        )
        {
            try
            {
                var categories = await _context
                    .Categories.Include(c => c.Name)
                    .OrderBy(c => c.Name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var responses = categories.Select(MapToResponse);
                return ApiResponse<IEnumerable<CategoryResponse>>.SuccessResponse(responses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all categories");
                return ApiResponse<IEnumerable<CategoryResponse>>.ErrorResponse(
                    "An error occurred while retrieving categories."
                );
            }
        }

        private static CategoryResponse MapToResponse(Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
            };
        }
    }

    public class CreateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }
    }

    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
