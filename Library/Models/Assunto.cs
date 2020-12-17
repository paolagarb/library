using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Assunto
    {
        public int Id { get; set; }
        [Display(Name="Assunto")]
        public string Nome { get; set; }
        public virtual List<LivroAssunto> LivroAssunto { get; set; }
        public Assunto()
        {

        }
    }
}
