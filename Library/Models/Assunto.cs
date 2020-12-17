using System.Collections.Generic;

namespace Library.Models
{
    public class Assunto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public virtual List<LivroAssunto> LivroAssunto { get; set; }
        public Assunto()
        {

        }
    }
}
