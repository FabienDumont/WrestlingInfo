﻿using Microsoft.EntityFrameworkCore;
using WrestlingInfo.API.Entities;

namespace WrestlingInfo.API.DbContexts;

public class WrestlingContext : DbContext {
	public DbSet<Promotion> Promotions { get; set; }
	public DbSet<Wrestler> Wrestlers { get; set; }
	public DbSet<WrestlingEvent> WrestlingEvents { get; set; }
	public DbSet<WrestlingEventReview> WrestlingEventReviews { get; set; }

	public WrestlingContext(DbContextOptions<WrestlingContext> options) : base(options) {
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.Entity<Promotion>().HasData(
			new Promotion("WWE") {
				Id = 1
			}
		);
		
		modelBuilder.Entity<WrestlingEvent>().HasData(
			new WrestlingEvent("Raw") {
				Id = 1,
				PromotionId = 1,
				Date = new DateOnly(2024, 1, 1)
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