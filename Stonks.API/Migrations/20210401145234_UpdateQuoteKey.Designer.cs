﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Stonks.API.Data;

namespace Stonks.API.Migrations
{
    [DbContext(typeof(StonksContext))]
    [Migration("20210401145234_UpdateQuoteKey")]
    partial class UpdateQuoteKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Stonks.API.Models.Quote", b =>
                {
                    b.Property<string>("Symbol")
                        .HasColumnType("text");

                    b.Property<decimal>("Change")
                        .HasColumnType("numeric");

                    b.Property<string>("ChangePercent")
                        .HasColumnType("text");

                    b.Property<decimal>("High")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("LatestTradingDay")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("Low")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Open")
                        .HasColumnType("numeric");

                    b.Property<decimal>("PreviousClose")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<long>("Volume")
                        .HasColumnType("bigint");

                    b.HasKey("Symbol");

                    b.ToTable("Quotes");
                });

            modelBuilder.Entity("Stonks.API.Models.Stock", b =>
                {
                    b.Property<string>("Symbol")
                        .HasColumnType("text");

                    b.Property<string>("AssetType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Sector")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Symbol");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("Stonks.API.Models.TimeSeries", b =>
                {
                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Symbol")
                        .HasColumnType("text");

                    b.Property<decimal>("Close")
                        .HasColumnType("numeric");

                    b.Property<decimal>("High")
                        .HasColumnType("numeric");

                    b.Property<string>("Interval")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Low")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Open")
                        .HasColumnType("numeric");

                    b.Property<string>("StockSymbol")
                        .HasColumnType("text");

                    b.Property<int>("Volume")
                        .HasColumnType("integer");

                    b.HasKey("TimeStamp", "Symbol");

                    b.HasIndex("StockSymbol");

                    b.ToTable("TimeSeries");
                });

            modelBuilder.Entity("Stonks.API.Models.TimeSeries", b =>
                {
                    b.HasOne("Stonks.API.Models.Stock", "Stock")
                        .WithMany()
                        .HasForeignKey("StockSymbol");

                    b.Navigation("Stock");
                });
#pragma warning restore 612, 618
        }
    }
}
