namespace Library.Models
{
    public class LivroAssunto
    {
        public int Id { get; set; }
        public virtual Livro Livro { get; set; }
        public int LivroId { get; set; }
        public virtual Assunto Assunto { get; set; }
        public int AssuntoId { get; set; }
        public LivroAssunto()
        {
            
        }
    }
}
