namespace PetAdoptionSystemApi.DTOs.Pet
{
    public class PetResponseDto
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public bool IsAdopted { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
