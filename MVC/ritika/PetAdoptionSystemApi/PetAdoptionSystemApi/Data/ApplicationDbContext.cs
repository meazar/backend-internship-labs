using Microsoft.EntityFrameworkCore;
using PetAdoptionSystemApi.Models;
namespace PetAdoptionSystemApi.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Adoption> Adoptions { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);
            modelBuilder.Entity<Pet>()
                .HasKey(p => p.PetId);
            modelBuilder.Entity<Adoption>()
                .HasOne(a => a.Pet)
                .WithMany(p => p.Adoptions)
                .HasForeignKey(a => a.PetId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Adoption>()
                .HasOne(a => a.User)
                .WithMany(u => u.Adoptions)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();
        }
    }
}
