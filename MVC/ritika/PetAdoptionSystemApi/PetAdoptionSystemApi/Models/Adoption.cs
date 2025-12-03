namespace PetAdoptionSystemApi.Models
{
    public class Adoption
    {
        public int AdoptionId { get; set; }
        public int PetId { get; set; }
        public Pet Pet { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime AdoptionDate { get; set; }
        public Adoption()
        {
            AdoptionDate = DateTime.Now;
        }
        public override string ToString()
        {
            return $"{AdoptionId}: Pet {PetId} adopted by User {UserId} on {AdoptionDate.ToShortDateString()}";
        }
    }
}
