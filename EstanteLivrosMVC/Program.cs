using Controllers;
using Models;

internal class Program
{
    public static Shelf MyShelf { get; set; }
    public static MongoController mc = new MongoController();

    private static void Main(string[] args)
    {
        MyShelf = new Shelf();
        MyShelf.books = mc.LoadMongoShelf();

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
        } while (opt != 3);
    }

    private static void Menu()
    {
        Console.Clear();
        Console.WriteLine("Menu de opções: ");
        Console.WriteLine("1 - Adicionar livro");
        Console.WriteLine("2 - Visualizar estante");
        Console.WriteLine("3 - Sair do menu");
    }

    private static void ChooseOption(int opt)
    {
        bool validation = true;
        switch (opt)
        {
            case 1: validation = InsertBook(); break;

            case 2: validation = PrintShelf(); break;
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

        Console.WriteLine("Nome da editora: ");
        string publisher = Console.ReadLine();

        Book book = ShelfController.GetBook(MyShelf, title, author);

        if (book is not null)
        {
            Console.WriteLine("Livro já existente na estante.");
            return false;
        }

        book = BookController.InsertBook(title, author, publisher);
        ShelfController.InsertBookShelf(MyShelf, book);
        mc.InsertBook(book);
        return true;
    }

    private static bool PrintShelf()
    {
        Console.Clear();

        Console.WriteLine("1 - Mostrar a estante completa");
        Console.WriteLine("2 - Mostrar livros guardados");
        Console.WriteLine("3 - Mostrar livros emprestados");
        Console.WriteLine("4 - Sair do menu");
        int.TryParse(Console.ReadLine(), out int opt);

        Console.WriteLine();
        switch (opt)
        {
            case 1:
                if (ShelfController.ShelfCount(MyShelf) == 0)
                {
                    PrintError("Estante vazia.");
                    return true;
                }

                ShelfController.PrintShelf(MyShelf);
                ShelfOperations(MyShelf);
                break;

            case 2:
                Shelf storedBooks = new Shelf();
                storedBooks.books = MyShelf.books.FindAll(book => book.Status);

                if (ShelfController.ShelfCount(storedBooks) == 0)
                {
                    PrintError("Estante vazia.");
                    return true;
                }

                ShelfController.PrintShelf(storedBooks);
                ShelfOperations(storedBooks);
                break;

            case 3:
                Shelf borrowBooks = new Shelf();
                borrowBooks.books = MyShelf.books.FindAll(book => !book.Status);

                if (ShelfController.ShelfCount(borrowBooks) == 0)
                {
                    PrintError("Estante vazia.");
                    return true;
                }

                ShelfController.PrintShelf(borrowBooks);
                ShelfOperations(borrowBooks);
                break;

            case 4: return true;

            default:
                Console.WriteLine("Opção inválida");
                return false;
        }

        return true;
    }

    private static void ShelfOperations(Shelf shelf)
    {
        Console.WriteLine();
        Console.WriteLine("Operações disponíveis: ");
        Console.WriteLine("1 - Editar livro");
        Console.WriteLine("2 - Remover livro");
        Console.WriteLine("3 - Sair do menu ");

        int.TryParse(Console.ReadLine(), out int opt);
        Book? book = null;

        switch (opt) 
        {
            case 1:
                book = FindBook(shelf);
                if (book is not null) EditBook(book);
                else PrintError("Livro não encontrado");
                break;
            case 2:
                book = FindBook(shelf);
                if (book is not null) ShelfController.RemoveBook(MyShelf, book);
                else PrintError("Livro não encontrado");
                break;
        }
    }

    private static Book? FindBook(Shelf shelf)
    {
        Console.WriteLine("Insira o index do livro: ");
        bool valid = int.TryParse(Console.ReadLine(), out int index);

        if (!valid) return null;

        return ShelfController.GetBookByIndex(shelf, index);
    }

    private static void EditBook(Book book)
    {
        Console.Clear();
        Console.WriteLine(book);
        Console.WriteLine();

        Console.WriteLine("O que deseja fazer?");
        Console.WriteLine("1 - Editar titulo");
        Console.WriteLine("2 - Editar autor");
        Console.WriteLine("3 - Mudar status");
        Console.WriteLine("4 - Sair");

        int.TryParse(Console.ReadLine(), out int opt);
        switch (opt)
        {
            case 1:
                Console.WriteLine("Escreva o novo titulo: ");
                string title = Console.ReadLine();
                book = BookController.ChangeBookTitle(book, title);
                break;
            case 2:
                Console.WriteLine("Escreva o novo autor: ");
                string author = Console.ReadLine();
                book = BookController.ChangeBookAuthor(book, author);
                break;
            case 3:
                book = BookController.ChangeBookStatus(book);
                break;
            case 4: return;
            default: PrintError("Opção inválida, tente novamente"); break;
        }
        ShelfController.EditShelf(MyShelf, book);
    }
}