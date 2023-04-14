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

        public void InsertBook(Book book)
        {
            // Filtro de conteúdo a ser encontrado
            var filter = Builders<BsonDocument>.Filter.Eq("_id", book.Id);

            // Verificação de duplicatas
            int count = 0;

            count = StoredCollection.Find(filter).ToList().Count;
            if (count != 0) return;

            count = BorrowCollection.Find(filter).ToList().Count;
            if (count != 0) return;

            // Insere na coleção correspondente
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

        public void EditBookTitle(Book book)
        {
            // Filtro de conteúdo
            var filter = Builders<BsonDocument>.Filter.Eq("_id", book.Id);
            // Builder de set
            var update = Builders<BsonDocument>.Update.Set("title", book.Title);
            // Filtro para verificação de titulo repetido
            var titleFilter = Builders<BsonDocument>.Filter.Eq("title", book.Title);
            var authorFilter = Builders<BsonDocument>.Filter.Eq("author", book.Author);
            var verificationFilter = Builders<BsonDocument>.Filter.And(titleFilter, authorFilter);

            // Edita na coleção correspondente
            if (book.Status)
            {
                if (StoredCollection.Find(verificationFilter).Count() != 0) return;
                StoredCollection.UpdateOne(filter, update);
            }
            else
            {
                if (StoredCollection.Find(verificationFilter).Count() != 0) return;
                BorrowCollection.UpdateOne(filter, update);
            }
        }

        public void EditBookAuthor(Book book)
        {
            // Filtro de conteúdo
            var filter = Builders<BsonDocument>.Filter.Eq("_id", book.Id);
            // Builder de set
            var update = Builders<BsonDocument>.Update.Set("author", book.Author);

            var titleFilter = Builders<BsonDocument>.Filter.Eq("title", book.Title);
            var authorFilter = Builders<BsonDocument>.Filter.Eq("author", book.Author);
            var verificationFilter = Builders<BsonDocument>.Filter.And(titleFilter, authorFilter);

            // Edita na coleção correspondente
            if (book.Status)
            {
                if (StoredCollection.Find(verificationFilter).Count() != 0) return;
                StoredCollection.UpdateOne(filter, update);
            }
            else
            {
                if (StoredCollection.Find(verificationFilter).Count() != 0) return;
                BorrowCollection.UpdateOne(filter, update);
            }
        }

        public void EditBookPublisher(Book book)
        {
            // Filtro de conteúdo
            var filter = Builders<BsonDocument>.Filter.Eq("_id", book.Id);
            // Builder de set
            var update = Builders<BsonDocument>.Update.Set("publisher", book.Publisher);

            if (book.Status) StoredCollection.UpdateOne(filter, update);
            else BorrowCollection.UpdateOne(filter, update);
        }

        public void EditBookStatus(Book book)
        {
            // Filtro de conteúdo
            var filter = Builders<BsonDocument>.Filter.Eq("_id", book.Id);

            // Insere na coleção contrária da atual, e remove documento da atual coleção
            if (book.Status)
            {
                BorrowCollection.FindOneAndDelete(filter);
                StoredCollection.InsertOne(new BsonDocument
                {
                    {"_id", book.Id},
                    { "title", book.Title },
                    { "author", book.Author },
                    {"publisher", book.Publisher },
                    {"status", book.Status }
                });
            }

            else
            {
                StoredCollection.FindOneAndDelete(filter);
                BorrowCollection.InsertOne(new BsonDocument
                {
                    {"_id", book.Id},
                    { "title", book.Title },
                    { "author", book.Author },
                    {"publisher", book.Publisher },
                    {"status", book.Status }
                });
            }
        }

        public void RemoveBook(Book book)
        {
            // Filtro de conteúdo
            var filter = Builders<BsonDocument>.Filter.Eq("_id", book.Id);

            // Remove da coleção correspondente
            if (book.Status) StoredCollection.FindOneAndDelete(filter);
            else BorrowCollection.FindOneAndDelete(filter);
        }

    }
}
