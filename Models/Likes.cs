﻿using System.Text.Json.Serialization;

namespace YappingAPI.Models
{
    public class Likes
    {
        [JsonPropertyName("objid")]
        public string ObjId { get; set; }
        [JsonPropertyName("userid")]
        public string UserId { get; set; }
        [JsonPropertyName("isliked")]
        public bool IsLiked { get; set; }
    }
}
