using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Livro
    {
        public int Id { get; set; }
        [Display(Name ="Título")]
        public string Titulo { get; set; }
        [Display(Name ="Edição")]
        public int Edicao { get; set; }
        public int Ano { get; set; }
        public virtual Editora Editora { get; set; }
        [Display(Name="Editora")]
        public int EditoraId { get; set; }
        public virtual List<LivroAutor> LivroAutor { get; set; }
        public virtual List<LivroAssunto> LivroAssunto { get; set; }


        public Livro()
        {

        }
    }
}
