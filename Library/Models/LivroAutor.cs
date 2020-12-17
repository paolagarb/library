namespace Library.Models
{
    public class LivroAutor
    {
        public int Id { get; set; }
        public virtual Livro Livro { get; set; }
        public int LivroId { get; set; }
        public virtual Autor Autor { get; set; }
        public int AutorId { get; set; }

        public LivroAutor()
        {

        }
    }
}
