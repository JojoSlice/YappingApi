using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace YappingAPI.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]

        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("userid")]
        public string UserId { get; set; }
        [JsonPropertyName("categoryid")]
        public string CategoryId { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("imgpath")]
        public string? ImgPath { get; set; }
        [JsonPropertyName("createdat")]
        public DateTime CreatedAt { get; set; }

        public Post() { }

        public Post(string userId, string categoryId, string text, string title, string? imgPath = null)
        {
            UserId = userId;
            CategoryId = categoryId;
            Text = text;
            Title = title;
            ImgPath = imgPath;
        }
    }
}
