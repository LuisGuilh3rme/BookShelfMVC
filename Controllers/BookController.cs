using Models;

namespace Controllers
{
    public static class BookController
    {
        public static Book InsertBook(string title, string author)
        {
            Book book = new Book();
            book.Id = Guid.NewGuid();
            book.Title = title;
            book.Author = author;
            book.Status = true;

            return book;
        }
    }
}