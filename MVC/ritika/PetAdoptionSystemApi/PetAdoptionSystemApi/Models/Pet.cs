namespace PetAdoptionSystemApi.Models
{
    public class Pet
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public bool IsAdopted { get; set; }
        public DateTime DateAdded { get; set; }
        public ICollection<Adoption> Adoptions { get; set; }
        public Pet()
        {
           IsAdopted = false;
            DateAdded = DateTime.Now;
        }
       
    }
}
