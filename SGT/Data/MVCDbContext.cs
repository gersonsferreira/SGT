using Microsoft.EntityFrameworkCore;
using SGT.Models.Domain;

namespace SGT.Data
{
    public class MVCDbContext : DbContext
    {
        public MVCDbContext (DbContextOptions<MVCDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tarefas> Tarefas { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
