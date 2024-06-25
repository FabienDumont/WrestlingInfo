using WrestlingInfo.API.Entities;

namespace WrestlingInfo.API.Services;

public interface IWrestlingInfoRepository {
	Task<IEnumerable<Promotion>> GetPromotionsAsync();
	Task<Promotion?> GetPromotionAsync(int promotionId, bool includeWrestlingEvents);
	Task<IEnumerable<WrestlingEvent>> GetWrestlingEventsForPromotionAsync(int promotionId);
	Task<WrestlingEvent?> GetWrestlingEventForPromotionAsync(int promotionId, int wrestlingEventId, bool includeReviews);
	Task<IEnumerable<WrestlingEventReview>> GetReviewsForWrestlingEventAsync(int promotionId, int wrestlingEventId);
	Task<WrestlingEventReview?> GetReviewForWrestlingEventAsync(int promotionId, int wrestlingEventId, int reviewId);
	
	Task<IEnumerable<Wrestler>> GetWrestlersAsync();
	Task<Wrestler?> GetWrestlerAsync(int wrestlerId);
}