using CodeTask2.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeTask2
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Note> Notes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("User ID=morrison;Password=1234;Server=localhost;Port=5432;Database=task2Db;Integrated Security=true;Pooling=true;");
        }
    }
}