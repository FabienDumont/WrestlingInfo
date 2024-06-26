using AutoMapper;
using WrestlingInfo.API.Entities;
using WrestlingInfo.API.Models;

namespace WrestlingInfo.API.Profiles;

public class WrestlingEventReviewProfile : Profile {
	public WrestlingEventReviewProfile() {
		CreateMap<WrestlingEventReview, WrestlingEventReviewDto>();
		CreateMap<WrestlingEventReviewForCreationDto, WrestlingEventReview>();
		CreateMap<WrestlingEventReviewForUpdateDto, WrestlingEventReview>();
		CreateMap<WrestlingEventReview, WrestlingEventReviewForUpdateDto>();
	}
}