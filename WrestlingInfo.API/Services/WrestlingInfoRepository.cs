using Microsoft.EntityFrameworkCore;
using WrestlingInfo.API.DbContexts;
using WrestlingInfo.API.Entities;

namespace WrestlingInfo.API.Services;

public class WrestlingInfoRepository : IWrestlingInfoRepository {
	private readonly WrestlingInfoContext _context;

	public WrestlingInfoRepository(WrestlingInfoContext context) {
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task<IEnumerable<Promotion>> GetPromotionsAsync() {
		return await _context.Promotions.OrderBy(p => p.Name).ToListAsync();
	}

	public async Task<(IEnumerable<Promotion>, PaginationMetadata)> GetPromotionsAsync(string? name, string? searchQuery, int pageNumber, int pageSize) {
		var collection = _context.Promotions as IQueryable<Promotion>;

		if (!string.IsNullOrWhiteSpace(name)) {
			name = name.Trim();
			collection = collection.Where(p => p.Name == name);
		}

		if (!string.IsNullOrWhiteSpace(searchQuery)) {
			searchQuery = searchQuery.Trim();
			collection = collection.Where(p => p.Name.Contains(searchQuery) || p.Description != null && p.Description.Contains(searchQuery));
		}

		var totalItemCount = await collection.CountAsync();

		var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

		var collectionToReturn = await collection.OrderBy(p => p.Name).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();

		return (collectionToReturn, paginationMetadata);
	}

	public async Task<Promotion?> GetPromotionAsync(int promotionId, bool includeWrestlingEvents) {
		if (includeWrestlingEvents) {
			return await _context.Promotions.Include(p => p.Events).Where(p => p.Id == promotionId).FirstOrDefaultAsync();
		}

		return await _context.Promotions.Where(p => p.Id == promotionId).FirstOrDefaultAsync();
	}

	public async Task<bool> PromotionExistsAsync(int promotionId) {
		return await _context.Promotions.AnyAsync(p => p.Id == promotionId);
	}

	public async Task<IEnumerable<WrestlingEvent>> GetWrestlingEventsForPromotionAsync(int promotionId) {
		return await _context.WrestlingEvents.Where(e => e.PromotionId == promotionId).ToListAsync();
	}

	public async Task<WrestlingEvent?> GetWrestlingEventAsync(int wrestlingEventId, bool includeReviews) {
		if (includeReviews) {
			return await _context.WrestlingEvents.Include(e => e.Reviews).Where(e => e.Id == wrestlingEventId).FirstOrDefaultAsync();
		}

		return await _context.WrestlingEvents.Where(e => e.Id == wrestlingEventId).FirstOrDefaultAsync();
	}

	public async Task<bool> WrestlingEventExistsForPromotionAsync(int promotionId, int wrestlingEventId) {
		return await _context.WrestlingEvents.AnyAsync(e => e.Id == wrestlingEventId && e.PromotionId == promotionId);
	}

	public async Task<IEnumerable<WrestlingEventReview>> GetReviewsForWrestlingEventAsync(int wrestlingEventId) {
		return await _context.WrestlingEventReviews.Where(r => r.WrestlingEventId == wrestlingEventId).ToListAsync();
	}

	public async Task<WrestlingEventReview?> GetReviewForWrestlingEventAsync(int wrestlingEventId, int reviewId) {
		return await _context.WrestlingEventReviews.Where(r => r.WrestlingEventId == wrestlingEventId && r.Id == reviewId).FirstOrDefaultAsync();
	}

	public async Task AddReviewForWrestlingEvent(int wrestlingEventId, WrestlingEventReview review) {
		var wrestlingEvent = await GetWrestlingEventAsync(wrestlingEventId, false);

		if (wrestlingEvent is not null) {
			wrestlingEvent.Reviews.Add(review);
		}
	}

	public void DeleteReview(WrestlingEventReview review) {
		_context.WrestlingEventReviews.Remove(review);
	}

	public async Task<IEnumerable<Wrestler>> GetWrestlersAsync() {
		return await _context.Wrestlers.OrderBy(w => w.Name).ToListAsync();
	}

	public async Task<Wrestler?> GetWrestlerAsync(int wrestlerId) {
		return await _context.Wrestlers.Where(w => w.Id == wrestlerId).FirstOrDefaultAsync();
	}

	public async Task<bool> SaveChangesAsync() {
		return (await _context.SaveChangesAsync() >= 0);
	}
}