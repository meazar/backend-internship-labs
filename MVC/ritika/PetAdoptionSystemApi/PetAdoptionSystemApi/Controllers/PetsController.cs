using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetAdoptionSystemApi.DTOs.Pagination;
using PetAdoptionSystemApi.DTOs.Pet;
using PetAdoptionSystemApi.Models;
using PetAdoptionSystemApi.Services.Interfaces;

namespace PetAdoptionSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly IMapper _mapper;
        public PetsController(IPetService petService, IMapper mapper)
        {
            _petService = petService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetResponseDto>>> GetAll()
        {
            var pets = await _petService.GetAllPetsAsync();
            return Ok(_mapper.Map<IEnumerable<PetResponseDto>>(pets));
        }
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<PetResponseDto>>> GetAvailablePets()
        {
            var pets = await _petService.GetAvailablePetsAsync();
            return Ok(_mapper.Map<IEnumerable<PetResponseDto>>(pets));

        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PetResponseDto>> GetById(int id)
        {
            var pet = await _petService.GetByIdAsync(id);
            if (pet == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PetResponseDto>(pet));
        }
        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] string? species, [FromQuery] string? gender, [FromQuery] bool? isAdopted, [FromQuery] PaginationParams pagination)
        {
            var pets = await _petService.GetAllPetsAsync();
            var query = pets.AsQueryable();
            if (!string.IsNullOrWhiteSpace(species))
            {
                query = query.Where(p => p.Species.ToLower() == species.Trim().ToLower());
            }
            if (!string.IsNullOrWhiteSpace(gender))
            {
                query = query.Where(p => p.Gender.ToLower() == gender.Trim().ToLower());
            }
            if (isAdopted.HasValue)
            {
                query = query.Where(p => p.IsAdopted == isAdopted.Value);
            }
            var totalCount = query.Count();

            var items = query
                .OrderByDescending(p=> p.DateAdded)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();

            var response = new
            {
                pagination.PageNumber,
                pagination.PageSize,
                totalCount,
                items = _mapper.Map<IEnumerable<PetResponseDto>>(items)
            };
            return Ok(response);

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<PetResponseDto>> Create(PetCreateDto dto)
        {
            var pet = _mapper.Map<Pet>(dto);
            await _petService.CreateAsync(pet);
            var response = _mapper.Map<PetResponseDto>(pet);
            return CreatedAtAction(nameof(GetById), new { id = response.PetId }, response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, PetCreateDto dto)
        {
            var existingPet = await _petService.GetByIdAsync(id);
            if (existingPet == null)
            {
                return NotFound();
            }
            _mapper.Map(dto, existingPet);
            await _petService.UpdateAsync(existingPet);
            return Ok("Pet updated successfully.");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingPet = await _petService.GetByIdAsync(id);
            if (existingPet == null)
            {
                return NotFound();
            }
            await _petService.DeleteAsync(id);
            return Ok("Pet Deleted successfully");
        }
    }
}
