using AutoMapper;
using WrestlingInfo.API.Entities;
using WrestlingInfo.API.Models;

namespace WrestlingInfo.API.Profiles;

public class WrestlerProfile : Profile {
	public WrestlerProfile() {
		CreateMap<Wrestler, WrestlerDto>();
	}
}