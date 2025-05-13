using Microsoft.AspNetCore.Mvc;

namespace YappingAPI.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class Post(Services.MongoDB mongoDb) : ControllerBase
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

        [HttpPost("post")]
        public async Task<IActionResult> CreatePost([FromBody] Models.Post post)
        {
            try
            {
                await _db.CreatePost(post);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
