﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TechChallenge.Infrastructure.Database;

#nullable disable

namespace TechChallenge.Infrastructure.Database.Migrations
{
    [DbContext(typeof(TechChallengeDbContext))]
    [Migration("20241207172700_AddedContactTable")]
    partial class AddedContactTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TechChallenge.Domain.Entities.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PhoneAreaId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PhoneAreaId");

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("TechChallenge.Domain.Entities.PhoneArea", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Code"));

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("PhoneArea");
                });

            modelBuilder.Entity("TechChallenge.Domain.Entities.Contact", b =>
                {
                    b.HasOne("TechChallenge.Domain.Entities.PhoneArea", "PhoneArea")
                        .WithMany()
                        .HasForeignKey("PhoneAreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PhoneArea");
                });
#pragma warning restore 612, 618
        }
    }
}