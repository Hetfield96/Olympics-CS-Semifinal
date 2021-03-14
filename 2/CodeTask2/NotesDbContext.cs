using CodeTask2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CodeTask2
{
    public class NotesDbContext : DbContext
    {
        private IConfiguration _configuration;
        public NotesDbContext(IConfiguration configuration, DbContextOptions<NotesDbContext> options)
            : base(options)
        {
            _configuration = configuration;
        }
        
        public DbSet<Note> Notes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}