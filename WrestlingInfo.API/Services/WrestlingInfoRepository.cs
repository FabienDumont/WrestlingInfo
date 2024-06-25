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
		return await _context.Promotions.OrderBy(c => c.Name).ToListAsync();
	}

	public async Task<Promotion?> GetPromotionAsync(int promotionId, bool includeWrestlingEvents) {
		if (includeWrestlingEvents) {
			return await _context.Promotions.Include(p => p.Events).Where(p => p.Id == promotionId).FirstOrDefaultAsync();
		}

		return await _context.Promotions.Where(p => p.Id == promotionId).FirstOrDefaultAsync();
	}

	public async Task<IEnumerable<WrestlingEvent>> GetWrestlingEventsForPromotionAsync(int promotionId) {
		return await _context.WrestlingEvents.Where(e => e.PromotionId == promotionId).ToListAsync();
	}

	public async Task<WrestlingEvent?> GetWrestlingEventForPromotionAsync(int promotionId, int wrestlingEventId, bool includeReviews) {
		if (includeReviews) {
			return await _context.WrestlingEvents.Include(e => e.Reviews).Where(e => e.PromotionId == promotionId && e.Id == wrestlingEventId)
				.FirstOrDefaultAsync();
		}

		return await _context.WrestlingEvents.Where(e => e.PromotionId == promotionId && e.Id == wrestlingEventId).FirstOrDefaultAsync();
	}

	public async Task<IEnumerable<WrestlingEventReview>> GetReviewsForWrestlingEventAsync(int promotionId, int wrestlingEventId) {
		return await _context.WrestlingEventReviews.Where(r => r.WrestlingEvent.PromotionId == promotionId && r.WrestlingEventId == wrestlingEventId)
			.ToListAsync();
	}

	public async Task<WrestlingEventReview?> GetReviewForWrestlingEventAsync(int promotionId, int wrestlingEventId, int reviewId) {
		return await _context.WrestlingEventReviews
			.Where(r => r.WrestlingEvent.PromotionId == promotionId && r.WrestlingEventId == wrestlingEventId && r.Id == reviewId).FirstOrDefaultAsync();
	}

	public async Task<IEnumerable<Wrestler>> GetWrestlersAsync() {
		return await _context.Wrestlers.OrderBy(w => w.Name).ToListAsync();
	}

	public async Task<Wrestler?> GetWrestlerAsync(int wrestlerId) {
		return await _context.Wrestlers.Where(w => w.Id == wrestlerId).FirstOrDefaultAsync();
	}
}