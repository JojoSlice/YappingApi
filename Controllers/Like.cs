using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YappingAPI.Controllers
{
    [Route("api/likes")]
    [ApiController]
    public class Like(Services.MongoDB db) : ControllerBase
    {
        private readonly Services.MongoDB _db = db;
        
        [HttpGet]
        public async Task<IActionResult> GetLikes(string objid)
        {
            var likes = await _db.GetLikes(objid);
            if(likes != null) { return Ok(likes); }
            else { return BadRequest("No likes found"); }
        }

        [HttpPost("{objid}/{userid}")]
        public async Task LikeObj(string objid, string userid)
        {
                await _db.Like(objid, userid);
        }
    }
}
