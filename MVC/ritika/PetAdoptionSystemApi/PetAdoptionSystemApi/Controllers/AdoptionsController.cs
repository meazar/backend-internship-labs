using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetAdoptionSystemApi.DTOs.Adoption;
using PetAdoptionSystemApi.Models;
using PetAdoptionSystemApi.Services.Interfaces;

namespace PetAdoptionSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptionsController : ControllerBase
    {
       private readonly IAdoptionService _adoptionService;
        private readonly IMapper _mapper;
         public AdoptionsController(IAdoptionService adoptionService, IMapper mapper)
         {
              _adoptionService = adoptionService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdoptionResponseDto>>> GetAll()
        {
            var items = await _adoptionService.GetAllAdoptionsAsync();
            return Ok(_mapper.Map<IEnumerable<AdoptionResponseDto>>(items));
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AdoptionResponseDto>> GetById(int id)
        {
            var item = await _adoptionService.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<AdoptionResponseDto>(item));
        }
        [HttpPost]
        public async Task<ActionResult<AdoptionResponseDto>> Create(AdoptionCreateDto dto)
        {
            try
            {
                var adoption = _mapper.Map<Adoption>(dto);
                await _adoptionService.CreateAsync(adoption);
                var response = _mapper.Map<AdoptionResponseDto>(adoption);
                return CreatedAtAction(nameof(GetById), new { id = response.AdoptionId }, response);

            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });

            }
        }
    }
}
