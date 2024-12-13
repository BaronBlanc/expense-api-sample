﻿// <auto-generated />
using System;
using ExpenseApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExpenseApi.Migrations
{
    [DbContext(typeof(ExpenseDbContext))]
    [Migration("20241213084238_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("ExpenseApi.Model.Currency", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Currencies");

                    b.HasData(
                        new
                        {
                            Id = new Guid("64bae1e5-9cce-45d4-8319-d1f3fdc6e835"),
                            Code = "USD",
                            Name = "U.S. Dollar",
                            Symbol = "$"
                        },
                        new
                        {
                            Id = new Guid("e7ac993d-a10b-493d-83fc-c7c780782fa5"),
                            Code = "RUB",
                            Name = "Russian Ruble",
                            Symbol = "₽"
                        });
                });

            modelBuilder.Entity("ExpenseApi.Model.Expense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CurrencyId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("UserId");

                    b.ToTable("Expenses");

                    b.HasData(
                        new
                        {
                            Id = new Guid("2d2f39ab-2523-4ae9-a911-89170f672939"),
                            Amount = 103.68933144408696,
                            Comment = "Business meeting",
                            CurrencyId = new Guid("64bae1e5-9cce-45d4-8319-d1f3fdc6e835"),
                            Date = new DateTime(2024, 12, 11, 8, 42, 38, 12, DateTimeKind.Utc).AddTicks(3440),
                            Type = 2,
                            UserId = new Guid("c0141cb5-03fe-43c2-9cfa-96b40d77f21b")
                        },
                        new
                        {
                            Id = new Guid("f6cda131-c002-422c-906d-aac7f3c0dc26"),
                            Amount = 118.39945875162243,
                            Comment = "Travel expense",
                            CurrencyId = new Guid("e7ac993d-a10b-493d-83fc-c7c780782fa5"),
                            Date = new DateTime(2024, 11, 17, 8, 42, 38, 12, DateTimeKind.Utc).AddTicks(4376),
                            Type = 1,
                            UserId = new Guid("b5c5e15b-e1c3-4d35-beef-7bf0994b38a4")
                        },
                        new
                        {
                            Id = new Guid("bcda063a-f719-44f9-8066-dd2827b3ec51"),
                            Amount = 140.62828957261399,
                            Comment = "Client dinner",
                            CurrencyId = new Guid("64bae1e5-9cce-45d4-8319-d1f3fdc6e835"),
                            Date = new DateTime(2024, 12, 11, 8, 42, 38, 12, DateTimeKind.Utc).AddTicks(4387),
                            Type = 0,
                            UserId = new Guid("c0141cb5-03fe-43c2-9cfa-96b40d77f21b")
                        },
                        new
                        {
                            Id = new Guid("15dd0dfd-728c-476c-89a6-2bc9bccbe75c"),
                            Amount = 113.81893995949065,
                            Comment = "Hotel stay",
                            CurrencyId = new Guid("e7ac993d-a10b-493d-83fc-c7c780782fa5"),
                            Date = new DateTime(2024, 12, 7, 8, 42, 38, 12, DateTimeKind.Utc).AddTicks(4391),
                            Type = 0,
                            UserId = new Guid("b5c5e15b-e1c3-4d35-beef-7bf0994b38a4")
                        });
                });

            modelBuilder.Entity("ExpenseApi.Model.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CurrencyId")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c0141cb5-03fe-43c2-9cfa-96b40d77f21b"),
                            CurrencyId = new Guid("64bae1e5-9cce-45d4-8319-d1f3fdc6e835"),
                            FirstName = "Anthony",
                            LastName = "Stark"
                        },
                        new
                        {
                            Id = new Guid("b5c5e15b-e1c3-4d35-beef-7bf0994b38a4"),
                            CurrencyId = new Guid("e7ac993d-a10b-493d-83fc-c7c780782fa5"),
                            FirstName = "Natasha",
                            LastName = "Romanova"
                        });
                });

            modelBuilder.Entity("ExpenseApi.Model.Expense", b =>
                {
                    b.HasOne("ExpenseApi.Model.Currency", "Currency")
                        .WithMany("Expenses")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ExpenseApi.Model.User", "User")
                        .WithMany("Expenses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ExpenseApi.Model.User", b =>
                {
                    b.HasOne("ExpenseApi.Model.Currency", "Currency")
                        .WithMany("Users")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("ExpenseApi.Model.Currency", b =>
                {
                    b.Navigation("Expenses");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("ExpenseApi.Model.User", b =>
                {
                    b.Navigation("Expenses");
                });
#pragma warning restore 612, 618
        }
    }
}
