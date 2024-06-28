using WrestlingInfo.API.Entities;

namespace WrestlingInfo.API.Services;

public interface IWrestlingInfoRepository {
	Task<IEnumerable<User>> GetUsersAsync();
	Task<User?> GetUserAsync(string userName, string password);
	Task<IEnumerable<Promotion>> GetPromotionsAsync();
	Task<(IEnumerable<Promotion>, PaginationMetadata)> GetPromotionsAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
	Task<Promotion?> GetPromotionAsync(int promotionId, bool includeWrestlingEvents);
	Task<bool> PromotionExistsAsync(int promotionId);
	Task<IEnumerable<WrestlingEvent>> GetWrestlingEventsForPromotionAsync(int promotionId);
	Task<WrestlingEvent?> GetWrestlingEventAsync(int wrestlingEventId, bool includeReviews);
	Task<bool> WrestlingEventExistsForPromotionAsync(int promotionId, int wrestlingEventId);
	Task<IEnumerable<WrestlingEventReview>> GetReviewsForWrestlingEventAsync(int wrestlingEventId);
	Task<WrestlingEventReview?> GetReviewForWrestlingEventAsync(int wrestlingEventId, int reviewId);
	Task AddReviewForWrestlingEvent(int wrestlingEventId, WrestlingEventReview review);
	void DeleteReview(WrestlingEventReview review);
	Task<IEnumerable<Wrestler>> GetWrestlersAsync();
	Task<Wrestler?> GetWrestlerAsync(int wrestlerId);
	Task<bool> SaveChangesAsync();
}