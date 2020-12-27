using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Library.Models.ViewModels
{
    public class Livros
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public List<string> Autores { get; set; }
        public int Edicao { get; set; }
        public int Ano { get; set; }
        public string Editora { get; set; }
        public List<string> Assuntos { get; set; }
        public FileStreamResult Foto { get; set; }

        public Livros()
        {

        }
    }
}
