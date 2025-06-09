using Microsoft.AspNetCore.Mvc;

namespace YappingAPI.Controllers
{
    [ApiController]
    [Route("api/message")]
    public class MessageController(Services.MongoDB db) : Controller
    {
        private readonly Services.MongoDB _db = db;

        [HttpGet]
        public async Task<IActionResult> ReciveMessages(string id)
        {
            var messages = await _db.GetResivedMessages(id);
            if(messages == null) return NotFound();

            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody]Models.Message message)
        {
            try
            {
                await _db.SendMessage(message);
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
