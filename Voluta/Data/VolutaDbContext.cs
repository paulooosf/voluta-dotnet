using Microsoft.EntityFrameworkCore;
using Voluta.Models;

namespace Voluta.Data
{
    public class VolutaDbContext : DbContext
    {
        public VolutaDbContext(DbContextOptions<VolutaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ong> Ongs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.OngsVoluntario)
                .WithMany(o => o.Voluntarios)
                .UsingEntity(j => j.ToTable("UsuariosOngs"));
        }
    }
} 