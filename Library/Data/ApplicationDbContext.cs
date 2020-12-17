using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Library.Models.Livro> Livro { get; set; }
        public DbSet<Library.Models.Autor> Autor { get; set; }
        public DbSet<Library.Models.Editora> Editora { get; set; }
        public DbSet<Library.Models.Assunto> Assunto { get; set; }
        public DbSet<Library.Models.LivroAssunto> LivroAssunto { get; set; }
        public DbSet<Library.Models.LivroAutor> LivroAutor { get; set; }
    }
}
