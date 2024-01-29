using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tickets.Models;

namespace Tickets.Services
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cargo>().HasData(
                    new Cargo { Id = 1, Nome = "Funcionario" },
                    new Cargo { Id = 2, Nome = "Atendente" });

            modelBuilder.Entity<Chamado>().Property(p => p.Id).UseIdentityAlwaysColumn();
            modelBuilder.Entity<Comentario>().Property(p => p.Id).UseIdentityAlwaysColumn();
            modelBuilder.Entity<Status>().HasData(
                    new Status { Id = 1, Nome = "Aguardando" },
                    new Status { Id = 2, Nome = "Pausado" },
                    new Status { Id = 3, Nome = "Concluído" });
            
        }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
