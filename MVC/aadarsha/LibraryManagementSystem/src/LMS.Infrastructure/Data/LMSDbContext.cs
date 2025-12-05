using LMS.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Data
{
    public class LMSDbContext : DbContext
    {
        public LMSDbContext(DbContextOptions<LMSDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }

        public DbSet<Member> Members { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();

                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);

                entity.Property(u => u.LastName).HasMaxLength(100);

                entity.Property(u => u.Username).IsRequired().HasMaxLength(100);

                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);

                entity.Property(u => u.Password).IsRequired().HasMaxLength(255);

                entity.Property(u => u.RefreshToken).HasMaxLength(500);
                entity.Property(u => u.RefreshTokenExpiry);

                entity
                    .Property(u => u.Role)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("Member");

                entity.Property(u => u.IsActive).HasDefaultValue(true);

                entity.Property(u => u.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

                entity.Property(u => u.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            });

            // Member configuration
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasIndex(m => m.MemberId).IsUnique().HasFilter("[MemberId] IS NOT NULL");
                entity.HasIndex(m => m.UserId).IsUnique();

                entity.Property(m => m.MemberId).HasMaxLength(20);
                entity.Property(m => m.MembershipType).HasMaxLength(20).HasDefaultValue("Regular");
                entity.Property(m => m.MaxBooksAllowed).HasDefaultValue(5);
                entity.Property(m => m.CurrentBooksCheckedOut).HasDefaultValue(0);
                entity.Property(m => m.TotalFines).HasDefaultValue(0m);
                entity.Property(m => m.IsMembershipActive).HasDefaultValue(true);
                entity.Property(m => m.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
                entity.Property(m => m.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            });

            // Author configuration
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasIndex(a => new { a.FirstName, a.LastName }).IsUnique();

                entity.Property(a => a.FirstName).IsRequired().HasMaxLength(100);

                entity.Property(a => a.LastName).IsRequired().HasMaxLength(100);

                entity.Property(a => a.Biography).HasMaxLength(2000);

                entity.Property(a => a.Email).HasMaxLength(255);

                entity.Property(a => a.IsActive).HasDefaultValue(true);

                entity.Property(a => a.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

                entity.Property(a => a.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            });

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasIndex(c => c.Name).IsUnique();

                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);

                entity.Property(c => c.Description).HasMaxLength(500);

                entity.Property(c => c.IsActive).HasDefaultValue(true);

                entity.Property(c => c.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

                entity.Property(c => c.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            });

            // Book configuration
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasIndex(b => b.ISBN).IsUnique().HasFilter("[ISBN] IS NOT NULL");

                entity.Property(b => b.Title).IsRequired().HasMaxLength(500);

                entity.Property(b => b.ISBN).HasMaxLength(20);

                entity.Property(b => b.Description).HasMaxLength(1000);

                entity.Property(b => b.Publisher).HasMaxLength(200);

                entity.Property(b => b.Language).HasMaxLength(50).HasDefaultValue("English");

                entity.Property(b => b.CoverImageUrl).HasMaxLength(1000);

                entity.Property(b => b.TotalCopies).IsRequired().HasDefaultValue(1);

                entity.Property(b => b.AvailableCopies).IsRequired().HasDefaultValue(1);

                entity.Property(b => b.PublicationYear);

                entity.Property(b => b.IsActive).HasDefaultValue(true);

                entity.Property(b => b.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

                entity.Property(b => b.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            });

            // Transaction configuration
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(t => t.Status).HasMaxLength(20).HasDefaultValue("CheckedOut");
                entity.Property(t => t.Notes).HasMaxLength(500);
                entity.Property(t => t.LateFee).HasDefaultValue(0m);
                entity.Property(t => t.RenewalCount).HasDefaultValue(0);
                entity.Property(t => t.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
                entity.Property(t => t.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

                // Relationships
                entity
                    .HasOne(t => t.Member)
                    .WithMany(m => m.Transactions)
                    .HasForeignKey(t => t.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(t => t.Book)
                    .WithMany(b => b.Transactions)
                    .HasForeignKey(t => t.BookId)
                    .OnDelete(DeleteBehavior.Restrict);

                // One-to-One with Fine
                entity
                    .HasOne(t => t.Fine)
                    .WithOne(f => f.Transaction)
                    .HasForeignKey<Fine>(f => f.TransactionId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Fine configuration
            modelBuilder.Entity<Fine>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(f => f.Reason).HasMaxLength(20).HasDefaultValue("Overdue");
                entity.Property(f => f.Description).HasMaxLength(500);
                entity.Property(f => f.Status).HasMaxLength(20).HasDefaultValue("Pending");
                entity.Property(f => f.Amount).HasDefaultValue(0m);
                entity.Property(f => f.IsActive).HasDefaultValue(true);
                entity.Property(f => f.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
                entity.Property(f => f.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

                // Relationships
                entity
                    .HasOne(f => f.Member)
                    .WithMany(m => m.Fines)
                    .HasForeignKey(f => f.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Reservation configuration
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(r => r.Status).HasMaxLength(20).HasDefaultValue("Pending");
                entity.Property(r => r.Notes).HasMaxLength(500);
                entity.Property(r => r.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
                entity.Property(r => r.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

                // Relationships
                entity
                    .HasOne(r => r.Member)
                    .WithMany(m => m.Reservations)
                    .HasForeignKey(r => r.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(r => r.Book)
                    .WithMany(b => b.Reservations)
                    .HasForeignKey(r => r.BookId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // BookAuthor junction table configuration
            modelBuilder.Entity<BookAuthor>(entity =>
            {
                entity.HasKey(ba => ba.Id);
                entity.Property(ba => ba.Id).ValueGeneratedOnAdd();

                // Composite unique constraint
                entity.HasIndex(ba => new { ba.BookId, ba.AuthorId }).IsUnique();

                entity.Property(ba => ba.Role).HasMaxLength(50).HasDefaultValue("Author");

                entity.Property(ba => ba.Order).HasDefaultValue(1);

                entity.Property(ba => ba.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

                // Relationships
                entity
                    .HasOne(ba => ba.Book)
                    .WithMany(b => b.BookAuthors)
                    .HasForeignKey(ba => ba.BookId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasOne(ba => ba.Author)
                    .WithMany(a => a.BookAuthors)
                    .HasForeignKey(ba => ba.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // BookCategory junction table configuration
            modelBuilder.Entity<BookCategory>(entity =>
            {
                entity.HasKey(bc => bc.Id);
                entity.Property(bc => bc.Id).ValueGeneratedOnAdd();

                // Composite unique constraint
                entity.HasIndex(bc => new { bc.BookId, bc.CategoryId }).IsUnique();

                entity.Property(bc => bc.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");

                // Relationships
                entity
                    .HasOne(bc => bc.Book)
                    .WithMany(b => b.BookCategories)
                    .HasForeignKey(bc => bc.BookId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasOne(bc => bc.Category)
                    .WithMany()
                    .HasForeignKey(bc => bc.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
