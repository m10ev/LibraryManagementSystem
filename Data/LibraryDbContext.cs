using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class LibraryDbContext : DbContext
    {
        public virtual DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<BorrowedBook> BorrowedBooks { get; set; }

        // Constructor that accepts DbContextOptions
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public LibraryDbContext()
        {
        }

        // Default constructor for configuring directly (in case not using DI)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Check if options were already configured
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=LibraryDb;Integrated Security=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite primary key for BorrowedBook
            modelBuilder.Entity<BorrowedBook>()
                .HasKey(bb => new { bb.BookID, bb.MemberID });

            // Unique constraint on ISBN
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.ISBN)
                .IsUnique();

            // Explicit relationship mapping

            /// Book - Author
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorID)
                .OnDelete(DeleteBehavior.Cascade);

            /// BorrowedBook - Book
            modelBuilder.Entity<BorrowedBook>()
                .HasOne(bb => bb.Book)
                .WithMany(b => b.BorrowedBooks)
                .HasForeignKey(bb => bb.BookID)
                .OnDelete(DeleteBehavior.Cascade);

            /// BorrowedBook - Member
            modelBuilder.Entity<BorrowedBook>()
                .HasOne(bb => bb.Member)
                .WithMany(m => m.BorrowedBooks)
                .HasForeignKey(bb => bb.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            // ENUMS stored as strings
            modelBuilder.Entity<Book>()
                .Property(b => b.Genre)
                .HasConversion<string>();
        }
    }
}