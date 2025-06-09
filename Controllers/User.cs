using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YappingAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class User(Services.MongoDB mongoDb) : ControllerBase
    {
        private readonly Services.MongoDB _db = mongoDb;

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (await _db.LogIn(request.Username, request.Password))
            {
                return Ok();
            }
            return Unauthorized();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Models.User newUser)
        {
            try
            {
                await _db.RegisterUser(newUser);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("Userinfo")]
        public async Task<IActionResult> GetUsernameById(string id)
        {
            Console.WriteLine("Get Userinfo");
            var user = await _db.GetUserFromId(id);
            if (user != null)
            {
                return Ok(new { userid = user.Id, username = user.Username, profileImg = user.ProfileImg });
            }
            else
                return BadRequest("No user found");
        }

        

        [HttpGet("GetUserId")]
        public async Task<IActionResult> GetUserFromUsername(string username)
        {
            Console.WriteLine(username);
            if (username != null || username != string.Empty)
            {
                var userid = await _db.GetUserId(username);
                Console.WriteLine(userid + " i controller");
                if (userid != null) return Ok(userid);
            }
            return Unauthorized(new {message = "User not found"});
        }

        [HttpGet("GetUserFromId")]
        public async Task<IActionResult> GetUserFromId(string userId)
        {
            Console.WriteLine("User from id körs");
            if (userId != string.Empty)
            {
                Models.User user = await _db.GetUserFromId(userId);
                if (user != null) return Ok(user);
            }
            return Unauthorized(new { message = "User not authenticated." });
        }

        [HttpGet("changePic")]
        public async Task<IActionResult> ChangePic(string id, string path)
        {
            Console.WriteLine("changepic");
            try
            {
                await _db.UpdateProfilePic(id, path);
                Console.WriteLine("bild updaterad");
                return Ok();
            }
            catch
            {
                return BadRequest("Error");
            }
        }

        [HttpGet("usernameTaken")]
        public async Task<IActionResult> IsUsernameTaken(string username)
        {
            if (await _db.IsUsernameTaken(username)) return Ok();
            else return BadRequest("Username Taken");
        }

        [HttpGet("emailTaken")]
        public async Task<IActionResult> IsEmailTaken(string email)
        {
            if (await _db.IsEmailTaken(email)) return Ok();
            else return BadRequest("Email Taken");
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
