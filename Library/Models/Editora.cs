using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Editora
    {
        public int Id { get; set; }
        [Display(Name="Editora")]
        public string Nome { get; set; }
        public Editora()
        {

        }
    }
}
