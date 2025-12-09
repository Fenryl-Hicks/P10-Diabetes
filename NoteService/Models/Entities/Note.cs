using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NoteService.Models.Entities
{
    public class Note
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("patId")]
        public required int PatientId { get; set; }

        [BsonElement("note")]
        public required string Content { get; set; }

        [BsonElement("isDeleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
