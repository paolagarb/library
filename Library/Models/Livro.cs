using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Livro
    {
        public int Id { get; set; }
        [Display(Name = "Título")]
        public string Titulo { get; set; }
        [Display(Name = "Edição")]
        public int Edicao { get; set; }
        public int Ano { get; set; }
        public virtual Editora Editora { get; set; }
        [Display(Name = "Editora")]
        public int EditoraId { get; set; }
        [Display(Name = "Autor")]
        public virtual List<LivroAutor> LivroAutor { get; set; }
        [Display(Name = "Assunto")]
        public virtual List<LivroAssunto> LivroAssunto { get; set; }
        public byte[] Dados { get; set; }
        public string ContentType { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
        public string IdentityUserId { get; set; }

        public Livro()
        {

        }
    }
}
