using System.Text.Json.Serialization;

namespace YappingAPI.Models
{
    public class UserInfo
    {
        [JsonPropertyName("userid")]
        public string UserId { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("profileimg")]
        public string ProfileImg { get; set; }
    }
}
