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
        [HttpGet("getpost")]
        public async Task<IActionResult> GetPost(string postid)
        {
                var post = await _db.GetPost(postid);
            if (post != null)
                return Ok(post);
            else return BadRequest("No Post found");
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
        [HttpPost("{postid}like")]
        public async Task<IActionResult> LikePost(string postid)
        {
            try
            {
                await _db.AddPostLike(postid);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fel i LikePost: " + ex.Message);
                return BadRequest("Could not like post");
            }
        }
    }
}
