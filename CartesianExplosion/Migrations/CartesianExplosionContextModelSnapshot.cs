﻿// <auto-generated />
using System;
using CartesianExplosion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CartesianExplosion.Migrations
{
    [DbContext(typeof(CartesianExplosionContext))]
    partial class CartesianExplosionContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("CartesianExplosion.Payin", b =>
                {
                    b.Property<long>("PayinId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("AmountGoingIn")
                        .HasColumnType("numeric");

                    b.Property<decimal>("BalanceAfter")
                        .HasColumnType("numeric");

                    b.Property<decimal>("BalanceBefore")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("PayinDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("SummaryId")
                        .HasColumnType("bigint");

                    b.Property<long?>("TransactionId")
                        .HasColumnType("bigint");

                    b.HasKey("PayinId");

                    b.HasIndex("SummaryId");

                    b.HasIndex("TransactionId");

                    b.ToTable("Payins");
                });

            modelBuilder.Entity("CartesianExplosion.Payout", b =>
                {
                    b.Property<long>("PayoutId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("AmountGoingOut")
                        .HasColumnType("numeric");

                    b.Property<decimal>("BalanceAfter")
                        .HasColumnType("numeric");

                    b.Property<decimal>("BalanceBefore")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("PayoutDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("SummaryId")
                        .HasColumnType("bigint");

                    b.Property<long?>("TransactionId")
                        .HasColumnType("bigint");

                    b.HasKey("PayoutId");

                    b.HasIndex("SummaryId");

                    b.HasIndex("TransactionId");

                    b.ToTable("Payouts");
                });

            modelBuilder.Entity("CartesianExplosion.Summary", b =>
                {
                    b.Property<long>("SummaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("SummaryId");

                    b.ToTable("Summaries");
                });

            modelBuilder.Entity("CartesianExplosion.Transaction", b =>
                {
                    b.Property<long>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("Load1")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Load2")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Load3")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Load4")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Load5")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Load6")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Load7")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Load8")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Load9")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("TransactionId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("CartesianExplosion.Payin", b =>
                {
                    b.HasOne("CartesianExplosion.Summary", null)
                        .WithMany("payins")
                        .HasForeignKey("SummaryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CartesianExplosion.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId");
                });

            modelBuilder.Entity("CartesianExplosion.Payout", b =>
                {
                    b.HasOne("CartesianExplosion.Summary", null)
                        .WithMany("payouts")
                        .HasForeignKey("SummaryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CartesianExplosion.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId");
                });
#pragma warning restore 612, 618
        }
    }
}
