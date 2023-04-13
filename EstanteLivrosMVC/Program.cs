using System;
using System.ComponentModel.Design;
using Controllers;
using Models;

internal class Program
{
    public static Shelf MyShelf { get; set; }

    private static void Main(string[] args)
    {
        MyShelf = new Shelf();

        int opt = 0;
        do
        {
            Menu();
            Console.WriteLine("Digite a opção desejada e tecle ENTER: ");
            int.TryParse(Console.ReadLine(), out opt);

            if (opt == 0)
            {
                PrintError("Opção inválida, tente novamente!");
                continue;
            }

            ChooseOption(opt);
        } while (opt != 4);
    }

    private static void Menu()
    {
        Console.Clear();
        Console.WriteLine("Menu de opções: ");
        Console.WriteLine("1 - Adicionar livro");
        Console.WriteLine("2 - Remover livro");
        Console.WriteLine("3 - Visualizar estante");
        Console.WriteLine("4 - Sair do menu");
    }

    private static void ChooseOption(int opt)
    {
        bool validation = true;
        switch(opt)
        {
            case 1: validation = InsertBook(); break;

            case 3: validation = PrintShelf(); break;
        }

        if (!validation) PrintError("Insira informações válidas na próxima vez!");
    }

    private static void PrintError(string message)
    {
        Console.WriteLine(message);
        Console.WriteLine("Tecle ENTER para continuar: ");
        Console.ReadLine();
    }

    private static bool InsertBook()
    {
        Console.Clear();
        Console.WriteLine("Nome do livro: ");
        string title = Console.ReadLine();

        Console.WriteLine("Nome do autor: ");
        string author = Console.ReadLine();

        Book book = ShelfController.GetBook(MyShelf, title, author);

        if (book is null)
        {
            Console.WriteLine("Livro já existente na estante.");
            return false;
        }

        MyShelf = ShelfController.InsertBookShelf(MyShelf, BookController.InsertBook(title, author));
        return true;
    }

    private static bool PrintShelf()
    {
        Console.Clear();

        if (MyShelf.books.Count == 0)
        {
            Console.WriteLine("Estante vazia.");
            return false;
        }

        Console.WriteLine("1 - Mostrar a estante completa");
        Console.WriteLine("2 - Mostrar livros guardados");
        Console.WriteLine("3 - Mostrar livros emprestados");
        Console.WriteLine("4 - Sair do menu");
        int.TryParse(Console.ReadLine(), out int opt);
        if (opt == 0) Console.WriteLine("Opção inválida, tente novamente");

        return true;
    }
}