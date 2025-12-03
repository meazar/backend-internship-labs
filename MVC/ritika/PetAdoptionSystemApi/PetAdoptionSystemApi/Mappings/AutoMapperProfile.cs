using AutoMapper;
using PetAdoptionSystemApi.DTOs.Adoption;


using PetAdoptionSystemApi.DTOs.Pet;
using PetAdoptionSystemApi.DTOs.User;
using PetAdoptionSystemApi.Models;

namespace PetAdoptionSystemApi.Mappings
{
   
        public class AutoMapperProfile: Profile 
        {
            public AutoMapperProfile()
            {
                CreateMap<PetCreateDto, Pet>();
                CreateMap<Pet, PetResponseDto>();

                CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
                CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

                CreateMap<AdoptionCreateDto, Adoption>();
                CreateMap<Adoption, AdoptionResponseDto>()
                .ForMember(dest => dest.PetName, opt => opt.MapFrom(src => src.Pet.Name))
                .ForMember(dest => dest.AdopterName, opt => opt.MapFrom(src => src.User.FullName));
            }
        }
   
    
        
    
}
