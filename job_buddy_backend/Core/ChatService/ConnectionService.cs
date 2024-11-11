using AutoMapper;
using job_buddy_backend.Core.Interfaces.Chat;
using job_buddy_backend.DTO.ChatDto;
using job_buddy_backend.Models.ChatModel;
using job_buddy_backend.Models.DataContext;
using job_buddy_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace job_buddy_backend.Core.ChatService
{
    public class ConnectionService : IConnectionService
    {
        private readonly JobBuddyDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ConnectionService> _logger;

        public ConnectionService(JobBuddyDbContext context, IMapper mapper, ILogger<ConnectionService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<int>> SendConnectionRequestAsync(ConnectionRequestDto connectionRequest)
        {
            try
            {
                var connection = new Connection
                {
                    RequestorID = connectionRequest.RequestorID,
                    RequesteeID = connectionRequest.RequesteeID,
                    Status = ConnectionStatus.Pending
                };

                _context.Connections.Add(connection);
                await _context.SaveChangesAsync();

                return ApiResponse<int>.SuccessResponse(connection.ConnectionID, "Connection request sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending connection request");
                return ApiResponse<int>.FailureResponse("Failed to send connection request.");
            }
        }

        public async Task<ApiResponse<string>> AcceptConnectionRequestAsync(int connectionId)
        {
            try
            {
                var connection = await _context.Connections.FindAsync(connectionId);
                if (connection == null || connection.Status != ConnectionStatus.Pending)
                    return ApiResponse<string>.FailureResponse("Connection request not found or already processed.");

                connection.Status = ConnectionStatus.Accepted;
                connection.AcceptedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return ApiResponse<string>.SuccessResponse("Connection request accepted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accepting connection request");
                return ApiResponse<string>.FailureResponse("Failed to accept connection request.");
            }
        }

        public async Task<ApiResponse<string>> RejectConnectionRequestAsync(int connectionId)
        {
            try
            {
                var connection = await _context.Connections.FindAsync(connectionId);
                if (connection == null || connection.Status != ConnectionStatus.Pending)
                    return ApiResponse<string>.FailureResponse("Connection request not found or already processed.");

                connection.Status = ConnectionStatus.Rejected;
                await _context.SaveChangesAsync();
                return ApiResponse<string>.SuccessResponse("Connection request rejected successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting connection request");
                return ApiResponse<string>.FailureResponse("Failed to reject connection request.");
            }
        }

        public async Task<ApiResponse<string>> RemoveConnectionAsync(int connectionId)
        {
            try
            {
                var connection = await _context.Connections.FindAsync(connectionId);
                if (connection == null || connection.Status != ConnectionStatus.Accepted)
                    return ApiResponse<string>.FailureResponse("Connection not found or already inactive.");

                connection.Status = ConnectionStatus.Blocked;
                await _context.SaveChangesAsync();
                return ApiResponse<string>.SuccessResponse("Connection removed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing connection");
                return ApiResponse<string>.FailureResponse("Failed to remove connection.");
            }
        }

        public async Task<ApiResponse<List<ConnectionResponseDto>>> GetConnectionsForUserAsync(int userId)
        {
            try
            {
                var connections = await _context.Connections
                    .Where(c => (c.RequestorID == userId || c.RequesteeID == userId) && c.Status == ConnectionStatus.Accepted)
                    .ToListAsync();

                var connectionDtos = _mapper.Map<List<ConnectionResponseDto>>(connections);
                return ApiResponse<List<ConnectionResponseDto>>.SuccessResponse(connectionDtos, "Connections retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving connections");
                return ApiResponse<List<ConnectionResponseDto>>.FailureResponse("Failed to retrieve connections.");
            }
        }

        public async Task<ApiResponse<List<ConnectionResponseDto>>> GetPendingFriendRequestsAsync()
        {
            var friendRequests = await _context.Connections
                .Where(c => c.Status == ConnectionStatus.Pending)
                .ToListAsync();

            var friendRequestDtos = _mapper.Map<List<ConnectionResponseDto>>(friendRequests);
            return ApiResponse<List<ConnectionResponseDto>>.SuccessResponse(friendRequestDtos, "Pending friend requests retrieved successfully.");
        }
    }
}
