using AutoMapper;
using job_buddy_backend.DTO.ChatDto;
using job_buddy_backend.Models.ChatModel;

namespace job_buddy_backend.DTO.Mapping
{
    public class MessagingProfile : Profile
    {
        public MessagingProfile()
        {
            // Chat mappings
            CreateMap<Chat, ChatDto.ChatDto>().ReverseMap();

            // Message mappings
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderID, opt => opt.MapFrom(src => src.SenderID))
                .ReverseMap();

            // Connection mappings
            CreateMap<Connection, ConnectionResponseDto>()
                .ForMember(dest => dest.RequestorID, opt => opt.MapFrom(src => src.RequestorID))
                .ForMember(dest => dest.RequesteeID, opt => opt.MapFrom(src => src.RequesteeID))
                .ReverseMap();

            // Additional configurations for requests
            CreateMap<ConnectionRequestDto, Connection>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ConnectionStatus.Pending))
                .ForMember(dest => dest.RequestedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
