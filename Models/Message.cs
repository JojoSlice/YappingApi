using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace YappingAPI.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("resiveid")]
        public string ResiveId { get; set; }

        [JsonPropertyName("sendeid")]
        public string SendId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("createdat")]
        public DateTime CreatedAt { get; set; }
        
        [JsonPropertyName("isread")]
        public bool IsRead { get; set; }
    }
}
