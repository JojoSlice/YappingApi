using Microsoft.AspNetCore.Mvc;

namespace YappingAPI.Controllers
{
    [ApiController]
    [Route("api/post")]
    public class Comment(Services.MongoDB mongoDb) : ControllerBase
    {
        private readonly Services.MongoDB _db = mongoDb;

        [HttpGet("latest")]
        public async Task<IActionResult> LatestPost(DateTime lastCreatedAt)
        {
            var post = await _db.GetLatestPosts(lastCreatedAt);
            if (post != null)
                return Ok(post);
            else return BadRequest("No posts found");
        }
    }
}
