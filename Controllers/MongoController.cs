using MongoDB.Driver;

using Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Controllers
{
    public class MongoController
    {
        MongoClient Mongo = new MongoClient("mongodb://localhost:27017/");
        IMongoDatabase Database { get; set; }
        IMongoCollection<BsonDocument> StoredCollection { get; set; }
        IMongoCollection<BsonDocument> BorrowCollection { get; set; }

        public MongoController() 
        {
            // Pegar database do mongo
            Database = Mongo.GetDatabase("shelf");

            // Pegar collections stored e borrow
            StoredCollection = Database.GetCollection<BsonDocument>("stored");
            BorrowCollection = Database.GetCollection<BsonDocument>("borrow");
        }

        public List<Book> LoadMongoShelf()
        {
            // Adicionar todos os documentos em uma lista
            List<BsonDocument> allBooks = StoredCollection.Find(new BsonDocument()).ToList();

            // Deserializa e armazena em uma lista do tipo livro
            List<Book> storedList = new List<Book>();
            allBooks.ForEach(book => storedList.Add(BsonSerializer.Deserialize<Book>(book)));

            // Adicionar todos os documentos em uma lista
            allBooks = BorrowCollection.Find(new BsonDocument()).ToList();

            // Deserializa e armazena em uma lista do tipo livro
            List<Book> borrowList = new List<Book>();
            allBooks.ForEach(book => borrowList.Add(BsonSerializer.Deserialize<Book>(book)));

            // Retorna uma lista com as duas outras listas concatenadas
            return new List<Book>().Concat(storedList.Concat(borrowList)).ToList();
        }

        public void InsertFullShelf(Shelf shelf)
        {
            shelf.books.ForEach(book =>
            {
                // Insere o novo documento na collection de livros guardados ou emprestados
                if (book.Status) StoredCollection.InsertOne(new BsonDocument
                {
                    { "title", book.Title },
                    { "author", book.Author },
                    {"publisher", book.Publisher },
                    {"status", book.Status }
                });

                else BorrowCollection.InsertOne(new BsonDocument
                {
                    { "title", book.Title },
                    { "author", book.Author },
                    {"publisher", book.Publisher },
                    {"status", book.Status }
                });
            });
        }

        public void InsertBook(Book book)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", book.Id);

            int count = 0;

            count = StoredCollection.Find(filter).ToList().Count;
            if (count != 0) return;

            count = BorrowCollection.Find(filter).ToList().Count;
            if (count != 0) return;

            if (book.Status) StoredCollection.InsertOne(new BsonDocument
                {
                    { "title", book.Title },
                    { "author", book.Author },
                    {"publisher", book.Publisher },
                    {"status", book.Status }
                });

            else BorrowCollection.InsertOne(new BsonDocument
                {
                    { "title", book.Title },
                    { "author", book.Author },
                    {"publisher", book.Publisher },
                    {"status", book.Status }
                });
        }
    }
}
