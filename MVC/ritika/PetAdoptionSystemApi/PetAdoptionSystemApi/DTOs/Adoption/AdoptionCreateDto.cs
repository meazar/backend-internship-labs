namespace PetAdoptionSystemApi.DTOs.Adoption
{
    public class AdoptionCreateDto
    {
        public int UserId { get; set; }
        public int PetId { get; set; }
        public DateTime AdoptionDate { get; set; }
    }
}
