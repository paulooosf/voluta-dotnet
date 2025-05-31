using Microsoft.EntityFrameworkCore;
using Voluta.Models;
using Voluta.Models.Auth;
using BC = BCrypt.Net.BCrypt;

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
                .HasIndex(u => u.Nome);

            modelBuilder.Entity<Ong>()
                .HasIndex(o => o.Nome);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Disponivel);

            modelBuilder.Entity<SolicitacaoVoluntariado>()
                .HasIndex(s => new { s.Status, s.DataSolicitacao });

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

            // Configurando o seed de dados
            var adminUser = new Usuario
            {
                Id = 1,
                Nome = "Admin",
                Email = "admin@voluta.com",
                Telefone = "(11) 99999-9999",
                Disponivel = false,
                DataCadastro = DateTime.Now,
                SenhaHash = BC.HashPassword("Admin@123"),
                Role = Roles.Admin,
                AreasInteresse = new[] { AreaAtuacao.EducacaoEnsino }
            };

            var representanteUser = new Usuario
            {
                Id = 2,
                Nome = "Representante ONG",
                Email = "representante@ong.com",
                Telefone = "(11) 88888-8888",
                Disponivel = false,
                DataCadastro = DateTime.Now,
                SenhaHash = BC.HashPassword("Representante@123"),
                Role = Roles.Representante,
                AreasInteresse = new[] { AreaAtuacao.EducacaoEnsino }
            };

            modelBuilder.Entity<Usuario>().HasData(adminUser, representanteUser);
        }
    }
} 