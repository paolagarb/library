using System.Collections.Generic;

namespace Library.Models
{
    public class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int Edicao { get; set; }
        public int Ano { get; set; }
        public virtual Editora Editora { get; set; }
        public int EditoraId { get; set; }
        public virtual List<LivroAutor> LivroAutor { get; set; }
        public virtual List<LivroAssunto> LivroAssunto { get; set; }


        public Livro()
        {

        }
    }
}
