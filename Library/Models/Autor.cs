using System.Collections.Generic;

namespace Library.Models
{
    public class Autor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<LivroAutor> LivroAutor { get; set; }

        public Autor()
        {

        }
    }
}
