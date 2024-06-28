using AutoMapper;
using WrestlingInfo.API.Entities;
using WrestlingInfo.API.Models;

namespace WrestlingInfo.API.Profiles;

public class UserProfile : Profile {
	public UserProfile() {
		CreateMap<User, UserDto>();
	}
}