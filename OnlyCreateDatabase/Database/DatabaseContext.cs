using Microsoft.EntityFrameworkCore;
using OnlyCreateDatabase.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlyCreateDatabase.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracja relacji User-Enrollment z kaskadowym usuwaniem
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User) // Enrollment ma jednego User
                .WithMany(u => u.Enrollments) // User ma wiele Enrollment
                .HasForeignKey(e => e.UserId) // Klucz obcy w Enrollment
                .OnDelete(DeleteBehavior.Restrict); // Usuwanie kaskadowe

            modelBuilder.Entity<Grade>()
               .HasOne(g => g.User)
               .WithMany()
               .HasForeignKey(g => g.UserId)
               .OnDelete(DeleteBehavior.Restrict); // Wyłącz kaskadowe usuwanie


        }
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Exercise> Exercise { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<FileUpload> Files { get; set; }
    }
}
