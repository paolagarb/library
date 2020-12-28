using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Assunto
    {
        public int Id { get; set; }
        [Display(Name="Assunto")]
        public string Nome { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
        public string IdentityUserId { get; set; }

        public Assunto()
        {

        }
    }
}
