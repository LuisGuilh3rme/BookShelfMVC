using Models;
using MongoDB.Bson;

namespace Controllers
{
    public static class BookController
    {
        public static Book InsertBook(string title, string author, string publisher)
        {
            Book book = new Book();
            book.Id = ObjectId.GenerateNewId();
            book.Title = title;
            book.Author = author;
            book.Publisher = publisher;
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
        public static Book ChangeBookPublisher(Book book, string publisher)
        {
            book.Publisher = publisher;
            return book;
        }

        public static Book ChangeBookStatus(Book book)
        {
            book.Status = (book.Status ? false : true);
            return book;
        }
    }
}