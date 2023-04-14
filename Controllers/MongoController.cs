using MongoDB.Driver;

using Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Controllers
{
    public class MongoController
    {
        MongoClient Mongo = new MongoClient("mongodb://localhost:27017/");

        public List<Book> LoadMongoShelf()
        {
            // Pegar database do mongo
            var database = Mongo.GetDatabase("shelf");

            // Pegar collections stored e borrow
            var collectionStored = database.GetCollection<BsonDocument>("stored");
            var collectionBorrow = database.GetCollection<BsonDocument>("borrow");

            // Adicionar todos os documentos em uma lista
            List<BsonDocument> allBooks = collectionStored.Find(new BsonDocument()).ToList();

            // Deserializa e armazena em uma lista do tipo livro
            List<Book> storedList = new List<Book>();
            allBooks.ForEach(book => storedList.Add(BsonSerializer.Deserialize<Book>(book)));

            // Adicionar todos os documentos em uma lista
            allBooks = collectionBorrow.Find(new BsonDocument()).ToList();

            // Deserializa e armazena em uma lista do tipo livro
            List<Book> borrowList = new List<Book>();
            allBooks.ForEach(book => borrowList.Add(BsonSerializer.Deserialize<Book>(book)));

            // Retorna uma lista com as duas outras listas concatenadas
            return new List<Book>().Concat(storedList.Concat(borrowList)).ToList();
        }
    }
}
