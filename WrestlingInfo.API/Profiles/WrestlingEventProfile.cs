using AutoMapper;
using WrestlingInfo.API.Entities;
using WrestlingInfo.API.Models;

namespace WrestlingInfo.API.Profiles;

public class WrestlingEventProfile : Profile {
	public WrestlingEventProfile() {
		CreateMap<WrestlingEvent, WrestlingEventWithoutReviewsDto>();
		CreateMap<WrestlingEvent, WrestlingEventDto>();
	}
}