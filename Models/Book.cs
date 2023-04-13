namespace Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool Status { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} | Titulo: {Title} | Autor: {Author} | Status: { (Status ? "Guardado" : "Emprestado") }";
        }
    }
}