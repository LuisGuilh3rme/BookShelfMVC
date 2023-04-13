using System.Threading.Channels;
using Models;
namespace Controllers
{
    public class ShelfController
    {
        public static Shelf InsertBookShelf(Shelf shelf, Book book)
        {
            shelf.books.Add(book);
            return shelf;
        }

        public static void PrintShelf(Shelf shelf)
        {
            int count = 0;
            shelf.books.ForEach(book => Console.WriteLine("{0}) {1}", ++count, book));
        }

        public static Book? GetBook (Shelf shelf, string title, string author) => shelf.books.Find(book => (book.Title == title) && (book.Author == author));

    }
}
