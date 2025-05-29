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
        public DbSet<SolicitacaoVoluntariado> SolicitacoesVoluntariado { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.OngsVoluntario)
                .WithMany(o => o.Voluntarios)
                .UsingEntity(j => j.ToTable("UsuariosOngs"));

            modelBuilder.Entity<SolicitacaoVoluntariado>()
                .HasOne(s => s.Usuario)
                .WithMany(u => u.SolicitacoesVoluntariado)
                .HasForeignKey(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SolicitacaoVoluntariado>()
                .HasOne(s => s.Ong)
                .WithMany(o => o.SolicitacoesVoluntariado)
                .HasForeignKey(s => s.OngId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Ong>()
                .HasIndex(o => o.Cnpj)
                .IsUnique();

            modelBuilder.Entity<Ong>()
                .HasIndex(o => o.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Email)
                .HasMaxLength(256);

            modelBuilder.Entity<Ong>()
                .Property(o => o.Email)
                .HasMaxLength(256);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Telefone)
                .HasMaxLength(15);

            modelBuilder.Entity<Ong>()
                .Property(o => o.Telefone)
                .HasMaxLength(15);
        }
    }
} 