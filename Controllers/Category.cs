using Microsoft.AspNetCore.Mvc;

namespace YappingAPI.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class Category(Services.MongoDB mongoDB) : ControllerBase
    {
        private readonly Services.MongoDB _dB = mongoDB;


        [HttpGet("get")]
        public async Task<IActionResult> GetCategory(string id)
        {
            var cat = await _dB.GetCategory(id);

            if (cat != null)
                return Ok(cat);
            else return BadRequest("No category found");
        }
    }
}
