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
            foreach (Book book in shelf.books)
            {
                Console.WriteLine(book);
            }
        }

        public static Book? GetBook (Shelf shelf, string title, string author) => shelf.books.Find(book => (book.Title == title) && (book.Author == author));

    }
}
