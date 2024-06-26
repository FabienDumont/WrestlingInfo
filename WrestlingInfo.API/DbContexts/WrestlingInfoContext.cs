using Microsoft.EntityFrameworkCore;
using WrestlingInfo.API.Entities;

namespace WrestlingInfo.API.DbContexts;

public class WrestlingInfoContext : DbContext {
	public DbSet<Promotion> Promotions { get; set; }
	public DbSet<Wrestler> Wrestlers { get; set; }
	public DbSet<WrestlingEvent> WrestlingEvents { get; set; }
	public DbSet<WrestlingEventReview> WrestlingEventReviews { get; set; }

	public WrestlingInfoContext(DbContextOptions<WrestlingInfoContext> options) : base(options) {
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.Entity<Promotion>().HasData(
			new Promotion("WWE") {
				Id = 1
			},
			new Promotion("AEW") {
				Id = 2
			}
		);
		
		modelBuilder.Entity<WrestlingEvent>().HasData(
			new WrestlingEvent("Raw") {
				Id = 1,
				PromotionId = 1,
				Date = new DateOnly(2024, 1, 1)
			},
			new WrestlingEvent("Rampage") {
				Id = 2,
				PromotionId = 2,
				Date = new DateOnly(2024, 1, 3)
			},
			new WrestlingEvent("Smackdown") {
				Id = 3,
				PromotionId = 1,
				Date = new DateOnly(2024, 1, 5)
			}
		);

		modelBuilder.Entity<Wrestler>().HasData(
			new Wrestler("AJ Styles") {
				Id = 1
			}, new Wrestler("Akira Tozawa") {
				Id = 2
			}, new Wrestler("Akam") {
				Id = 3
			}, new Wrestler("Alba Fyre") {
				Id = 4
			}, new Wrestler("Andrade") {
				Id = 5
			}
		);

		base.OnModelCreating(modelBuilder);
	}
}