using job_buddy_backend.Core.Interfaces.Chat;
using job_buddy_backend.DTO.ChatDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace job_buddy_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConnectionController : ControllerBase
    {
        private readonly IConnectionService _connectionService;
        private readonly ILogger<ConnectionController> _logger;

        public ConnectionController(IConnectionService connectionService, ILogger<ConnectionController> logger)
        {
            _connectionService = connectionService;
            _logger = logger;
        }

        [HttpPost("request")]
        public async Task<IActionResult> SendConnectionRequest([FromBody] ConnectionRequestDto connectionRequest)
        {
            var response = await _connectionService.SendConnectionRequestAsync(connectionRequest);
            return Ok(response);
        }

        [HttpPut("accept/{connectionId}")]
        public async Task<IActionResult> AcceptConnectionRequest(int connectionId)
        {
            var response = await _connectionService.AcceptConnectionRequestAsync(connectionId);
            return Ok(response);
        }

        [HttpPut("reject/{connectionId}")]
        public async Task<IActionResult> RejectConnectionRequest(int connectionId)
        {
            var response = await _connectionService.RejectConnectionRequestAsync(connectionId);
            return Ok(response);
        }

        [HttpDelete("remove/{connectionId}")]
        public async Task<IActionResult> RemoveConnection(int connectionId)
        {
            var response = await _connectionService.RemoveConnectionAsync(connectionId);
            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetConnections(int userId)
        {
            var response = await _connectionService.GetConnectionsForUserAsync(userId);
            return Ok(response);
        }

        [HttpGet("user/friend-requests")]
        public async Task<IActionResult> GetFriendRequests()
        {
            var response = await _connectionService.GetPendingFriendRequestsAsync();
            return Ok(response);
        }
    }
}
