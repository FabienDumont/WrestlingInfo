using AutoMapper;
using WrestlingInfo.API.Entities;
using WrestlingInfo.API.Models;

namespace WrestlingInfo.API.Profiles;

public class PromotionProfile : Profile {
	public PromotionProfile() {
		CreateMap<Promotion, PromotionWithoutWrestlingEventsDto>();
		CreateMap<Promotion, PromotionDto>();
	}
}