namespace PetAdoptionSystemApi.DTOs.Adoption
{
    public class AdoptionResponseDto
    {
        public int AdoptionId { get; set; }
        public int UserId { get; set; }
        public int PetId { get; set; }
        public string PetName { get; set; }
        public string AdopterName { get; set; }
        public DateTime AdoptionDate { get; set; }
    }
}
