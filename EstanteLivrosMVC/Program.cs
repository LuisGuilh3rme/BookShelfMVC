using Controllers;
using Models;

internal class Program
{
    private static void Main(string[] args)
    {
        int i = 0;
        Shelf shelf = new();

        do
        {
            i++;
            Console.WriteLine("Nome do livro: ");
            string title = Console.ReadLine();

            Console.WriteLine("Nome do autor: ");
            string author = Console.ReadLine();

            shelf = ShelfController.InsertBookShelf(shelf, BookController.InsertBook(title, author));
        } while (i < 3);

        ShelfController.PrintShelf(shelf);

        Console.WriteLine("Titulo do livro a ser encontrado: ");
        string findTitle = Console.ReadLine();

        Book book = ShelfController.GetBook(shelf, findTitle);
        
        if (book is not null) Console.WriteLine(book);
        else Console.WriteLine("Livro não encontrado");
    }
}