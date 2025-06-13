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
        public async Task<IActionResult> DeletePost([FromBody] DeletePostRequest request)
        {
            Console.WriteLine("DeletePost");
            try
            {
                await _dB.DeletePost(request.PostId);
                await _dB.DeleteRaport(request.RaportId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        public class DeletePostRequest
        {
            public string PostId { get; set; }
            public string RaportId { get; set; }
        }


        [HttpDelete("comment")]
        public async Task<IActionResult> DeleteComment([FromBody] DeleteCommentRequest request)
        {
            Console.WriteLine("DeleteComment");
            try
            {
                await _dB.DeleteComment(request.CommentId);
                await _dB.DeleteRaport(request.RaportId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        public class DeleteCommentRequest
        {
            public string CommentId { get; set; }
            public string RaportId { get; set; }
        }


        [HttpPut]
        public async Task<IActionResult> MarkReportAsRead([FromBody] MarkReportRequest request)
        {
            Console.WriteLine("MarkReportAsRead");
            try
            {
                await _dB.MarkReportAsRead(request.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        public class MarkReportRequest
        {
            public string Id { get; set; }
        }
    }
}
