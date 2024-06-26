using WrestlingInfo.API.Entities;

namespace WrestlingInfo.API.Services;

public interface IWrestlingInfoRepository {
	Task<IEnumerable<Promotion>> GetPromotionsAsync();
	Task<Promotion?> GetPromotionAsync(int promotionId, bool includeWrestlingEvents);
	Task<bool> PromotionExistsAsync(int promotionId);
	Task<IEnumerable<WrestlingEvent>> GetWrestlingEventsForPromotionAsync(int promotionId);
	Task<WrestlingEvent?> GetWrestlingEventForPromotionAsync(int promotionId, int wrestlingEventId, bool includeReviews);
	Task<bool> WrestlingEventExistsForPromotionAsync(int promotionId, int wrestlingEventId);
	Task<IEnumerable<WrestlingEventReview>> GetReviewsForWrestlingEventAsync(int wrestlingEventId);
	Task<WrestlingEventReview?> GetReviewForWrestlingEventAsync(int wrestlingEventId, int reviewId);
	Task AddReviewForWrestlingEvent(int promotionId, int wrestlingEventId, WrestlingEventReview review);
	void DeleteReview(WrestlingEventReview review);
	Task<IEnumerable<Wrestler>> GetWrestlersAsync();
	Task<Wrestler?> GetWrestlerAsync(int wrestlerId);
	Task<bool> SaveChangesAsync();
}