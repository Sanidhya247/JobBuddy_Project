using AutoMapper;
using job_buddy_backend.DTO.UserProfile;
using job_buddy_backend.Models;
using job_buddy_backend.Models.UserModel;
using JobBuddyBackend.Models;

namespace job_buddy_backend.DTO.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile() {
            CreateMap<RegisterUserDto, User>();
            CreateMap<User, RegisterUserDto>();
            CreateMap<LoginUserDto, User>();
            CreateMap<JobListing, JobListingDto>().ReverseMap();


            CreateMap<EmployerProfile, EmployerProfileDto>().ReverseMap();

            CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.ProfileCompletenessPercentage, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumbers != null));

            CreateMap<UpdateUserProfileDto, User>()
                .ForMember(dest => dest.Address, opt => opt.Condition(src => src.Address != null))
                .ForMember(dest => dest.DateOfBirth, opt => opt.Condition(src => src.DateOfBirth != null))
                .ForMember(dest => dest.Nationality, opt => opt.Condition(src => src.Nationality != null))
                .ForMember(dest => dest.LinkedInUrl, opt => opt.Condition(src => src.LinkedInUrl != null))
                .ForMember(dest => dest.PhoneNumbers, opt => opt.Ignore())  
                .ForMember(dest => dest.Educations, opt => opt.Ignore());
        }
    }
}
