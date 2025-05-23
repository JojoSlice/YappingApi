using Microsoft.AspNetCore.Mvc;

namespace YappingAPI.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class Post(Services.MongoDB mongoDb) : ControllerBase
    {
        private readonly Services.MongoDB _db = mongoDb;

        [HttpGet("latest")]
        public async Task<IActionResult> LatestPosts(DateTime lastCreatedAt)
        {
            var posts = await _db.GetLatestPosts(lastCreatedAt);
            if (posts != null)
                return Ok(posts);
            else return BadRequest("No posts found");
        }

        [HttpGet("getposts")]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _db.GetPosts();
            if (posts != null)
                return Ok(posts);
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
