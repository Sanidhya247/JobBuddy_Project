using AutoMapper;
using job_buddy_backend.Core.Interfaces.Chat;
using job_buddy_backend.DTO.ChatDto;
using job_buddy_backend.Models.ChatModel;
using job_buddy_backend.Models.DataContext;
using job_buddy_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace job_buddy_backend.Core.ChatService
{
    public class ChatService : IChatService
    {
        private readonly JobBuddyDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ChatService> _logger;

        public ChatService(JobBuddyDbContext context, IMapper mapper, ILogger<ChatService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<int>> CreateChatAsync(CreateChatDto chatDto)
        {
            try
            {
                var existingChat = await _context.Chats
                    .FirstOrDefaultAsync(c =>
                        c.JobSeekerID == chatDto.JobSeekerID &&
                        c.EmployerID == chatDto.EmployerID &&
                        c.IsActive);

                if (existingChat != null)
                {
                    return ApiResponse<int>.SuccessResponse(existingChat.ChatID, "Existing chat found.");
                }

                var chat = _mapper.Map<Chat>(chatDto);
                chat.IsActive = true;

                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();

                return ApiResponse<int>.SuccessResponse(chat.ChatID, "Chat created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating chat");
                return ApiResponse<int>.FailureResponse("Failed to create chat.");
            }
        }


        public async Task<ApiResponse<string>> SendMessageAsync(MessageDto messageDto)
        {
            try
            {
                var message = _mapper.Map<Message>(messageDto);
                message.SentAt = DateTime.UtcNow;

                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Message sent in ChatID: {messageDto.ChatID} by UserID: {messageDto.SenderID}");

                return ApiResponse<string>.SuccessResponse("Message sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message");
                return ApiResponse<string>.FailureResponse("Failed to send message.");
            }
        }

        public async Task<ApiResponse<string>> DeleteChatOnJobClosureOrUnfriendAsync(int jobId, int jobSeekerId, int employerId)
        {
            try
            {
                var chat = await _context.Chats
                    .Include(c => c.Messages)
                    .FirstOrDefaultAsync(c => c.JobID == jobId || (c.JobSeekerID == jobSeekerId && c.EmployerID == employerId));

                if (chat != null)
                {
                    chat.IsActive = false;
                    _context.Messages.RemoveRange(chat.Messages);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Chat deleted for JobID: {jobId}");
                    return ApiResponse<string>.SuccessResponse("Chat deleted due to job closure or unfriending.");
                }

                return ApiResponse<string>.FailureResponse("No active chat found to delete.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting chat");
                return ApiResponse<string>.FailureResponse("Failed to delete chat.");
            }
        }

        public async Task<ApiResponse<List<ChatDto>>> GetChatsAsync()
        {
            var chats = await _context.Chats
                .Include(c => c.Messages)
                .Include(c => c.JobSeeker)
                .Include(c => c.Employer)
                .ToListAsync();

            var chatDtos = _mapper.Map<List<ChatDto>>(chats);
            return ApiResponse<List<ChatDto>>.SuccessResponse(chatDtos, "Chats retrieved successfully.");
        }

        public async Task<ApiResponse<List<ChatDto>>> GetChatsAsync(int userID)
        {
            try
            {
                var chats = await _context.Chats
                    .Include(c => c.JobSeeker) 
                    .Include(c => c.Employer)
                    .Include(c => c.Messages.OrderByDescending(m => m.SentAt))
                    .Where(c => c.JobSeekerID == userID || c.EmployerID == userID)
                    .ToListAsync();

                var chatDtos = new List<ChatDto>();

                foreach (var chat in chats)
                {
                    //var isConnected = await _context.Connections
                    //    .AnyAsync(conn =>
                    //        (conn.RequestorID == chat.JobSeekerID && conn.RequesteeID == chat.EmployerID ||
                    //         conn.RequestorID == chat.EmployerID && conn.RequesteeID == chat.JobSeekerID) &&
                    //        conn.Status != ConnectionStatus.Blocked);

                    chatDtos.Add(new ChatDto
                    {
                        ChatID = chat.ChatID,
                        UserName = chat.JobSeekerID == userID ? chat.Employer.FullName : chat.JobSeeker.FullName,
                        LastMessage = chat.Messages.Any() ? chat.Messages.First().Content : "No messages yet",
                        LastMessageTime = chat.Messages.Any() ? chat.Messages.First().SentAt : (DateTime?)null,
                        IsActive = chat.IsActive 
                    });
                }

                return ApiResponse<List<ChatDto>>.SuccessResponse(chatDtos, "Chats retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching chats");
                return ApiResponse<List<ChatDto>>.FailureResponse("Failed to fetch chats.");
            }
        }



        public async Task<ApiResponse<List<MessageDto>>> GetMessagesAsync(int chatId)
        {
            var messages = await _context.Messages
                .Where(m => m.ChatID == chatId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            var messageDtos = _mapper.Map<List<MessageDto>>(messages);
            return ApiResponse<List<MessageDto>>.SuccessResponse(messageDtos, "Messages retrieved successfully.");
        }

        public async Task<ApiResponse<int>> GetUnreadCountsAsync()
        {
            
            var unreadCount = await _context.Messages
                .Where(m => !m.IsRead)
                .CountAsync();

            return ApiResponse<int>.SuccessResponse(unreadCount, "Unread count retrieved successfully.");
        }

        public async Task<ApiResponse<bool>> CheckConnectionStatusAsync(int userID, int employerID)
        {
            var isConnected = await _context.Connections
                .AnyAsync(c => (c.RequestorID == userID && c.RequesteeID == employerID) ||
                               (c.RequestorID == employerID && c.RequesteeID == userID) &&
                               c.Status == ConnectionStatus.Accepted);

            return ApiResponse<bool>.SuccessResponse(isConnected, "Connection status retrieved successfully.");
        }

    }
}
