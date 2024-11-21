using job_buddy_backend.DTO.ChatDto;
using job_buddy_backend.Models;

namespace job_buddy_backend.Core.Interfaces.Chat
{
    public interface IConnectionService
    {
        Task<ApiResponse<int>> SendConnectionRequestAsync(ConnectionRequestDto connectionRequest);
        Task<ApiResponse<string>> AcceptConnectionRequestAsync(int connectionId);
        Task<ApiResponse<string>> RejectConnectionRequestAsync(int connectionId);
        Task<ApiResponse<string>> RemoveConnectionAsync(int connectionId);
        Task<ApiResponse<List<ConnectionResponseDto>>> GetConnectionsForUserAsync(int userId);
        Task<ApiResponse<List<ConnectionResponseDto>>> GetPendingFriendRequestsAsync();
        Task<ApiResponse<ConnectionStatusResponseDto>> CheckConnectionStatusAsync(int requestorId, int requesteeId);
    }
}
