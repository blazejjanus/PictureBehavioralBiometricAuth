﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PictureBehavioralBiometricAuth.Db;

#nullable disable

namespace PictureBehavioralBiometricAuth.Db.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PictureBehavioralBiometricAuth.Db.Models.AuthImageModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("GridCellSize")
                        .HasColumnType("integer");

                    b.Property<int>("Height")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Width")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("AuthImages");
                });

            modelBuilder.Entity("PictureBehavioralBiometricAuth.Db.Models.AuthImageRegionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthImageModelId")
                        .HasColumnType("integer");

                    b.Property<int>("Height")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Width")
                        .HasColumnType("integer");

                    b.Property<int>("X")
                        .HasColumnType("integer");

                    b.Property<int>("Y")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AuthImageModelId");

                    b.ToTable("AuthImageRegions");
                });

            modelBuilder.Entity("PictureBehavioralBiometricAuth.Db.Models.AuthPointModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("X")
                        .HasColumnType("integer");

                    b.Property<int>("Y")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AuthPoints");
                });

            modelBuilder.Entity("PictureBehavioralBiometricAuth.Db.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthImageId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastLoginTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("RegistrationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.HasIndex("AuthImageId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PictureBehavioralBiometricAuth.Db.Models.AuthImageRegionModel", b =>
                {
                    b.HasOne("PictureBehavioralBiometricAuth.Db.Models.AuthImageModel", "AuthImageModel")
                        .WithMany("Regions")
                        .HasForeignKey("AuthImageModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AuthImageModel");
                });

            modelBuilder.Entity("PictureBehavioralBiometricAuth.Db.Models.AuthPointModel", b =>
                {
                    b.HasOne("PictureBehavioralBiometricAuth.Db.Models.UserModel", "User")
                        .WithMany("Points")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PictureBehavioralBiometricAuth.Db.Models.UserModel", b =>
                {
                    b.HasOne("PictureBehavioralBiometricAuth.Db.Models.AuthImageModel", "AuthImage")
                        .WithMany()
                        .HasForeignKey("AuthImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AuthImage");
                });

            modelBuilder.Entity("PictureBehavioralBiometricAuth.Db.Models.AuthImageModel", b =>
                {
                    b.Navigation("Regions");
                });

            modelBuilder.Entity("PictureBehavioralBiometricAuth.Db.Models.UserModel", b =>
                {
                    b.Navigation("Points");
                });
#pragma warning restore 612, 618
        }
    }
}
