using Microsoft.AspNetCore.Mvc;

namespace YappingAPI.Controllers
{
    [ApiController]
    [Route("api/report")]
    public class ReportController(Services.MongoDB mongoDB) : Controller
    {
        private readonly Services.MongoDB _dB = mongoDB;

        [HttpPost]
        public async Task<IActionResult> CreateRaport([FromBody] Models.Report report)
        {
            try
            {
                await _dB.CreateReport(report);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUnRead()
        {
            Console.WriteLine("GetUnRead");
            var reports = await _dB.GetAllUnreadReports();
            if(reports.Count > 0)
                return Ok(reports);
            else
                return NotFound();
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            Console.WriteLine("GetAllReports");

            var reports = await _dB.GetReports();
            if(reports.Count > 0)
                return Ok(reports);
            else
                return NotFound();
        }
        [HttpDelete("post")]
        public async Task<IActionResult> DeletePost(string id)
        {
            Console.WriteLine("DeletePost");
            try
            {
                await _dB.DeletePost(id);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("comment")]
        public async Task<IActionResult> DeleteComment(string id)
        {
            Console.WriteLine("DeleteComment");
            try
            {
                await _dB.DeleteComment(id);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> MarkReportAsRead(string id)
        {
            Console.WriteLine("MarkReportAsRead");
            try
            {
                await _dB.MarkReportAsRead(id);
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
