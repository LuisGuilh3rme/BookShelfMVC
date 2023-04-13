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

        public static Book ChangeBookTitle(Book book, string title)
        {
            book.Title = title;
            return book;
        }

        public static Book ChangeBookAuthor(Book book, string author)
        {
            book.Author = author;
            return book;
        }

        public static Book ChangeBookStatus(Book book)
        {
            book.Status = (book.Status ? false : true);
            return book;
        }
    }
}