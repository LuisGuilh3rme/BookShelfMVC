using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("publisher")]
        public string Publisher { get; set; }

        [BsonElement("status")]
        [BsonRepresentation(BsonType.Boolean)]
        public bool Status { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} | Titulo: {Title} | Autores: {Author} | Editora: {Publisher} | Status: {(Status ? "Guardado" : "Emprestado")}";
        }
    }
}