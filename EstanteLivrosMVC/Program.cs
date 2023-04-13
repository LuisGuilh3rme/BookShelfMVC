using Controllers;
using Models;

internal class Program
{
    private static void Main(string[] args)
    {
        Shelf shelf = new();

        Console.WriteLine("Nome do livro: ");
        string title = Console.ReadLine();

        Console.WriteLine("Nome do autor: ");
        string author = Console.ReadLine();

        shelf = ShelfController.InsertBookShelf(shelf, BookController.InsertBook(title, author));

        ShelfController.PrintShelf(shelf);
        
    }
}