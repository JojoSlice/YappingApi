using Microsoft.AspNetCore.Mvc;

namespace YappingAPI.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class Comment(Services.MongoDB mongo) : Controller
    {
        private readonly Services.MongoDB _db = mongo;

        [HttpGet("postcomments")]
        public async Task<IActionResult> GetPostComments(string postid)
        {
            var comments = await _db.GetCommentsOnPost(postid);
            if (comments != null)
                return Ok(comments);
            else return BadRequest("No Comments found");
        }
    }
}
