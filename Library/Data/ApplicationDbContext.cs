using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Library.Models;

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
    }
}
