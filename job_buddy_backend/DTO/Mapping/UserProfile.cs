using AutoMapper;
using job_buddy_backend.Models;
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


        }

    }
}
