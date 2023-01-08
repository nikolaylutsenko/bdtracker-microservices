using AutoMapper;
using BdTracker.Users.Dtos;
using BdTracker.Users.Dtos.Requests;
using BdTracker.Users.Dtos.Responses;
using BdTracker.Users.Entities;

namespace BirthdayTracker.Backend.Infrastructure
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            MapRequest();
            MapResponse();
        }
        private void MapRequest()
        {
            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
        }

        private void MapResponse()
        {
            CreateMap<User, UserResponse>();
        }
    }
}
