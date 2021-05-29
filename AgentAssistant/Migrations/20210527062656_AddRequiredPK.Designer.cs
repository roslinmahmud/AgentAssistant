﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AgentAssistant.Migrations
{
    [DbContext(typeof(AgentContext))]
    [Migration("20210527062656_AddRequiredPK")]
    partial class AddRequiredPK
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("Entities.Agent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("AgentId");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Agents");
                });

            modelBuilder.Entity("Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AgentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<short>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AgentId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("Entities.Volt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AgentId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("ClosingCashMoney")
                        .HasColumnType("REAL");

                    b.Property<double>("ClosingLiquidMoney")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<double>("OpeningCashMoney")
                        .HasColumnType("REAL");

                    b.Property<double>("OpeningLiquidMoney")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("AgentId");

                    b.ToTable("Volt");
                });

            modelBuilder.Entity("Entities.Employee", b =>
                {
                    b.HasOne("Entities.Agent", "Agent")
                        .WithMany("Employees")
                        .HasForeignKey("AgentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Agent");
                });

            modelBuilder.Entity("Entities.Volt", b =>
                {
                    b.HasOne("Entities.Agent", "Agent")
                        .WithMany("Volts")
                        .HasForeignKey("AgentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Agent");
                });

            modelBuilder.Entity("Entities.Agent", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Volts");
                });
#pragma warning restore 612, 618
        }
    }
}
