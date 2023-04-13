using Controllers;
using Models;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Nome do livro: ");
        string title = Console.ReadLine();

        Console.WriteLine("Nome do autor: ");
        string author = Console.ReadLine();

        Book book = BookController.InsertBook(title, author);

        Console.WriteLine(book);
    }
}