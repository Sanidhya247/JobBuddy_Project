using job_buddy_backend.Core.Interfaces.Chat;
using job_buddy_backend.DTO.ChatDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace job_buddy_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        [HttpGet("getChats")]
        public async Task<IActionResult> GetChats()
        {
            var response = await _chatService.GetChatsAsync();
            return Ok(response);
        }

        [HttpGet("getChats/{userID}")]
        public async Task<IActionResult> GetChats(int userID)
        {
            var response = await _chatService.GetChatsAsync(userID);
            return Ok(response);
        }

        [HttpGet("messages/{chatId}")]
        public async Task<IActionResult> GetMessages(int chatId)
        {
            var response = await _chatService.GetMessagesAsync(chatId);
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatDto chatDto)
        {
            var response = await _chatService.CreateChatAsync(chatDto);
            return Ok(response);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDto messageDto)
        {
            var response = await _chatService.SendMessageAsync(messageDto);
            return Ok(response);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteChat(int jobId, int jobSeekerId, int employerId)
        {
            var response = await _chatService.DeleteChatOnJobClosureOrUnfriendAsync(jobId, jobSeekerId, employerId);
            return Ok(response);
        }

        [HttpGet("unread-counts")]
        public async Task<IActionResult> GetUnreadCounts()
        {
            var response = await _chatService.GetUnreadCountsAsync();
            return Ok(response);
        }

        [HttpGet("check/{userID}/{employerID}")]
        public async Task<IActionResult> CheckConnectionStatus(int userID, int employerID)
        {
            var response = await _chatService.CheckConnectionStatusAsync(userID, employerID);
            return Ok(response);
        }
    }
}
