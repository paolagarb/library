using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Autor
    {
        public int Id { get; set; }
        [Display(Name="Autor")]
        public string Nome { get; set; }
        public List<LivroAutor> LivroAutor { get; set; }

        public Autor()
        {

        }
    }
}
