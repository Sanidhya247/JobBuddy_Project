﻿using job_buddy_backend.DTO.ChatDto;
using job_buddy_backend.Models;

namespace job_buddy_backend.Core.Interfaces.Chat
{
    public interface IChatService
    {
        Task<ApiResponse<int>> CreateChatAsync(ChatDto chatDto);
        Task<ApiResponse<string>> SendMessageAsync(MessageDto messageDto);
        Task<ApiResponse<string>> DeleteChatOnJobClosureOrUnfriendAsync(int jobId, int jobSeekerId, int employerId);
        Task<ApiResponse<List<ChatDto>>> GetChatsAsync();
        Task<ApiResponse<List<MessageDto>>> GetMessagesAsync(int chatId);
        Task<ApiResponse<int>> GetUnreadCountsAsync();
    }
}