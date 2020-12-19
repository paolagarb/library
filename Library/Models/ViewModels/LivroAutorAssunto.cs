using System.Collections.Generic;

namespace Library.Models.ViewModels
{
    public class LivroAutorAssunto
    {
        public Livro Livro { get; set; }
        public Autor Autor { get; set; }
        public Assunto Assunto { get; set; }
    }
}
