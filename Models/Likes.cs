using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace YappingAPI.Models
{
    public class Likes
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("objid")]
        public string ObjId { get; set; }
        [JsonPropertyName("userid")]
        public string UserId { get; set; }
        [JsonPropertyName("isliked")]
        public bool IsLiked { get; set; }
    }
}
