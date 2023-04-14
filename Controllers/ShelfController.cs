
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

        public static int ShelfCount(Shelf shelf) => shelf.books.Count();

        public static Book? GetBook(Shelf shelf, string title, string author) => shelf.books.Find(book => (book.Title == title) && (book.Author == author));

        public static Book? GetBookByIndex(Shelf shelf, int index)
        {
            index = index - 1;
            if (index > shelf.books.Count - 1 || index < 0) return null;

            return shelf.books[index];
        }

        public static void EditShelf(Shelf shelf, Book changedBook)
        {
            int index = shelf.books.FindIndex(book => book.Id == changedBook.Id);
            shelf.books[index] = changedBook;
        }

        public static void RemoveBook(Shelf shelf, Book book)
        {
            shelf.books.Remove(book);
        }
    }
}
