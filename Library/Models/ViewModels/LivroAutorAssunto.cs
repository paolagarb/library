using System.ComponentModel.DataAnnotations;

namespace Library.Models.ViewModels
{
    public class LivroAutorAssunto
    {
        public Livro Livro { get; set; }
        public Autor Autor { get; set; }
        [Display(Name="Gênero")]
        public Assunto Assunto { get; set; }
    }
}
