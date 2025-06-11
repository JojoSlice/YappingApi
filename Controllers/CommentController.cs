using Microsoft.AspNetCore.Mvc;

namespace YappingAPI.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentController(Services.MongoDB mongo) : Controller
    {
        private readonly Services.MongoDB _db = mongo;


        [HttpGet("comment")]
        public async Task<IActionResult> GetComment(string id)
        {
            var comment = await _db.GetComment(id);
            if(comment != null) 
                return Ok(comment);
            else
                return NotFound();
        }

        [HttpGet("postcomments")]
        public async Task<IActionResult> GetPostComments(string postid)
        {
            var comments = await _db.GetCommentsOnPost(postid);
            if (comments != null)
                return Ok(comments);
            else return BadRequest("No Comments found");
        }

        [HttpPost("new")]
        public async Task<IActionResult> PostNewComment([FromBody]Models.Comment comment)
        {
            try
            {
                await _db.CreateComment(comment);
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
