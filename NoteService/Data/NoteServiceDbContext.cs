using Microsoft.EntityFrameworkCore;
using NoteService.Models.Entities;

namespace NoteService.Data
{
    public class NoteServiceDbContext : DbContext
    {
        public NoteServiceDbContext(DbContextOptions<NoteServiceDbContext> options) : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Note>().HasQueryFilter(n => !n.IsDeleted);
        }
    }
}
