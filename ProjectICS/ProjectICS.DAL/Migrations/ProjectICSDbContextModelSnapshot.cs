﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectICS.DAL;

#nullable disable

namespace ProjectICS.DAL.Migrations
{
    [DbContext(typeof(ProjectICSDbContext))]
    partial class ProjectICSDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("ProjectICS.DAL.Entities.ActivityEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("ProjectICS.DAL.Entities.ProjectEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ProjectICS.DAL.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PictureLink")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ProjectEntityId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProjectEntityId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ProjectICS.DAL.Entities.ActivityEntity", b =>
                {
                    b.HasOne("ProjectICS.DAL.Entities.ProjectEntity", "Project")
                        .WithMany("ProjectActivities")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProjectICS.DAL.Entities.UserEntity", "User")
                        .WithMany("UserActivities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Project");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectICS.DAL.Entities.ProjectEntity", b =>
                {
                    b.HasOne("ProjectICS.DAL.Entities.UserEntity", "User")
                        .WithMany("UserProjects")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ProjectICS.DAL.Entities.UserEntity", b =>
                {
                    b.HasOne("ProjectICS.DAL.Entities.ProjectEntity", null)
                        .WithMany("ProjectUsers")
                        .HasForeignKey("ProjectEntityId");
                });

            modelBuilder.Entity("ProjectICS.DAL.Entities.ProjectEntity", b =>
                {
                    b.Navigation("ProjectActivities");

                    b.Navigation("ProjectUsers");
                });

            modelBuilder.Entity("ProjectICS.DAL.Entities.UserEntity", b =>
                {
                    b.Navigation("UserActivities");

                    b.Navigation("UserProjects");
                });
#pragma warning restore 612, 618
        }
    }
}
