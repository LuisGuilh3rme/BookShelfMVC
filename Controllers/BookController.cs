using Models;

namespace Controllers
{
    public class BookController: Book
    {
        public void InsertBook(int id, string title, string author)
        {
            Book book = new Book();
            book.Id = id;
            book.Title = title;
            book.Author = author;
        }
    }
}