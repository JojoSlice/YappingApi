using Microsoft.AspNetCore.Mvc;

namespace YappingAPI.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController(Services.MongoDB mongoDB) : Controller
    {
        private readonly Services.MongoDB _db = mongoDB;

        // GET api/chat?userId=123
        [HttpGet]
        public async Task<IActionResult> GetUserChats([FromQuery] string userId)
        {
            var chat = await _db.GetUsersChats(userId);

            if (chat != null)
                return Ok(chat);
            else
                return NotFound("No chat found");
        }

        // GET api/chat/messages?chatId=123
        [HttpGet("messages")]
        public async Task<IActionResult> GetChatMessages([FromQuery] string chatId)
        {
            var messages = await _db.GetChatMessages(chatId);

            if (messages != null)
                return Ok(messages);
            else 
                return NotFound("No messages found");
        }

        // POST api/chat/message
        [HttpPost("message")]
        public async Task<IActionResult> CreateChatMessage([FromBody] Models.ChatMessages message)
        {
            await _db.CreateChatMessage(message);
            return Ok("Message created");
        }

        // POST api/chat/group
        [HttpPost("group")]
        public async Task<IActionResult> CreateGroupChat([FromBody] Models.GroupChat groupChat)
        {
            await _db.CreateGroupChat(groupChat);
            return Ok("Group chat created");
        }

        // PUT api/chat/{chatId}/add-user/{userId}
        [HttpPut("{chatId}/add/{userId}")]
        public async Task<IActionResult> AddUserToChat(string chatId, string userId)
        {
            await _db.AddUserToChat(userId, chatId);
            return Ok("User added to chat");
        }

        // PUT api/chat/{chatId}/remove-user/{userId}
        [HttpPut("{chatId}/remove/{userId}")]
        public async Task<IActionResult> RemoveUserFromChat(string chatId, string userId)
        {
            await _db.RemoveUserFromChat(userId, chatId);
            return Ok("User removed from chat");
        }
    }

}
