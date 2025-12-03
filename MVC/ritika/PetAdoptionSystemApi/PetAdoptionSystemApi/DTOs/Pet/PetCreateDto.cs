namespace PetAdoptionSystemApi.DTOs.Pet
{
    public class PetCreateDto
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }
}
