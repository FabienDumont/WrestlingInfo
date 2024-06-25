﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WrestlingInfo.API.DbContexts;

#nullable disable

namespace WrestlingInfo.API.Migrations
{
    [DbContext(typeof(WrestlingContext))]
    partial class WrestlingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("WrestlingInfo.API.Entities.Promotion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Promotions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "WWE"
                        });
                });

            modelBuilder.Entity("WrestlingInfo.API.Entities.Wrestler", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Wrestlers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "AJ Styles"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Akira Tozawa"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Akam"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Alba Fyre"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Andrade"
                        });
                });

            modelBuilder.Entity("WrestlingInfo.API.Entities.WrestlingEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<int>("PromotionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PromotionId");

                    b.ToTable("WrestlingEvents");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Date = new DateOnly(2024, 1, 1),
                            Name = "Raw",
                            PromotionId = 1
                        });
                });

            modelBuilder.Entity("WrestlingInfo.API.Entities.WrestlingEventReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<double>("Rating")
                        .HasColumnType("REAL");

                    b.Property<int>("WrestlingEventId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WrestlingEventId");

                    b.ToTable("WrestlingEventReviews");
                });

            modelBuilder.Entity("WrestlingInfo.API.Entities.WrestlingEvent", b =>
                {
                    b.HasOne("WrestlingInfo.API.Entities.Promotion", "Promotion")
                        .WithMany("Events")
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Promotion");
                });

            modelBuilder.Entity("WrestlingInfo.API.Entities.WrestlingEventReview", b =>
                {
                    b.HasOne("WrestlingInfo.API.Entities.WrestlingEvent", "WrestlingEvent")
                        .WithMany("Reviews")
                        .HasForeignKey("WrestlingEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WrestlingEvent");
                });

            modelBuilder.Entity("WrestlingInfo.API.Entities.Promotion", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("WrestlingInfo.API.Entities.WrestlingEvent", b =>
                {
                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
