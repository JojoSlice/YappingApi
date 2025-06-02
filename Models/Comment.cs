using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace YappingAPI.Models
{
    public class Comment()
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("userid")]
        public string UserId { get; set; }
        [JsonPropertyName("postid")]
        public string PostId { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("createdat")]
        public DateTime CreatedAt { get; set; }
    }
}
