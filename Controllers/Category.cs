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
            Console.WriteLine("get cat: id: " + id);
            var cat = await _dB.GetCategoryById(id);


            if (cat != null)
            {
                Console.WriteLine(cat.Name);
                return Ok(cat);
            }
            else
            {
                Console.WriteLine("No category found");
                return BadRequest("No category found");
            }
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckCat(string name)
        {
            var cat = await _dB.GetCategory(name);
            if(cat != null) return Ok(cat);
            else
            {
                var newCat = new Models.Category { Name = name };
                await _dB.CreateCategory(newCat);
                    return Ok(newCat);  
            }
        }

        [HttpGet("allcategories")]
        public async Task<IActionResult> GetCategoties()
        {
            Console.WriteLine("all cattis");
            var cattis = await _dB.GetCategories();
            if (cattis != null)
            {
                Console.WriteLine(cattis.Count());
                return Ok(cattis);
            }
            else
                return BadRequest("No Categories found");
        }
    }
}
