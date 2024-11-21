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
                // Check if a connection already exists between the users
                var existingConnection = await _context.Connections
                    .FirstOrDefaultAsync(c =>
                        (c.RequestorID == connectionRequest.RequestorID && c.RequesteeID == connectionRequest.RequesteeID) ||
                        (c.RequestorID == connectionRequest.RequesteeID && c.RequesteeID == connectionRequest.RequestorID));

                // If a connection already exists, return the existing connection ID with appropriate status
                if (existingConnection != null)
                {
                    if (existingConnection.Status == ConnectionStatus.Pending)
                    {
                        return ApiResponse<int>.FailureResponse("A connection request is already pending between these users.");
                    }

                    else if (existingConnection.Status == ConnectionStatus.Accepted)
                    {
                        return ApiResponse<int>.FailureResponse("A connection already exists between these users.");
                    }

                    else
                    {
                        existingConnection.Status = ConnectionStatus.Pending;
                        await _context.SaveChangesAsync();
                        return ApiResponse<int>.SuccessResponse(existingConnection.ConnectionID, "Connection request sent successfully.");
                    }
                }

                // If no existing connection, create a new one
                var newConnection = new Connection
                {
                    RequestorID = connectionRequest.RequestorID,
                    RequesteeID = connectionRequest.RequesteeID,
                    Status = ConnectionStatus.Pending
                };

                _context.Connections.Add(newConnection);
                await _context.SaveChangesAsync();

                return ApiResponse<int>.SuccessResponse(newConnection.ConnectionID, "Connection request sent successfully.");
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
                var chat = await _context.Chats
                    .FirstOrDefaultAsync(c => c.JobID == connection.JobID || (c.JobSeekerID == connection.RequestorID && c.EmployerID == connection.RequesteeID));

                if (chat != null)
                {
                    chat.IsActive = true;
                    await _context.SaveChangesAsync();
                }
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
                var chat = await _context.Chats
                     .FirstOrDefaultAsync(c => c.JobID == connection.JobID || (c.JobSeekerID == connection.RequestorID && c.EmployerID == connection.RequesteeID));

                if (chat != null)
                {
                    chat.IsActive = false;
                    await _context.SaveChangesAsync();
                }
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

        public async Task<ApiResponse<ConnectionStatusResponseDto>> CheckConnectionStatusAsync(int requestorId, int requesteeId)
        {
            try
            {
                var connection = await _context.Connections
                    .FirstOrDefaultAsync(c =>
                        (c.RequestorID == requestorId && c.RequesteeID == requesteeId) ||
                        (c.RequestorID == requesteeId && c.RequesteeID == requestorId));

                if (connection == null)
                {
                    return ApiResponse<ConnectionStatusResponseDto>.SuccessResponse(
                        new ConnectionStatusResponseDto { Exists = false, Status = "None" },
                        "No connection exists."
                    );
                }

                return ApiResponse<ConnectionStatusResponseDto>.SuccessResponse(
                    new ConnectionStatusResponseDto { Exists = true, Status = connection.Status.ToString() },
                    "Connection status retrieved successfully."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking connection status");
                return ApiResponse<ConnectionStatusResponseDto>.FailureResponse("Failed to check connection status.");
            }
        }


    }
}
