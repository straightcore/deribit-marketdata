﻿// <auto-generated />
using System;
using MarketData.Adapter.Deribit.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MarketData.Adapter.Deribit.Migrations
{
    [DbContext(typeof(MarketDataDbContext))]
    [Migration("20200809132930_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6");

            modelBuilder.Entity("MarketData.Adapter.Deribit.EntityFramework.TradeDbo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<string>("BlockTradeId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Direction")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("IndexPrice")
                        .HasColumnType("TEXT");

                    b.Property<string>("InstrumentName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Liquidation")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("MarkPrice")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("TickDirection")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TradeId")
                        .HasColumnType("TEXT");

                    b.Property<int>("TradeSequence")
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("Volatility")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Trades");
                });
#pragma warning restore 612, 618
        }
    }
}
