﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlyCreateDatabase.Database;

#nullable disable

namespace OnlyCreateDatabase.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.3");

            modelBuilder.Entity("OnlyCreateDatabase.Model.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("createdAt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("updatedAt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("OnlyCreateDatabase.Model.Enrollment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsInCourse")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("UserId");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("OnlyCreateDatabase.Model.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DeadLine")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ExerciseDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExerciseName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("FileUploadId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("createdAt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("updatedAt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("FileUploadId");

                    b.ToTable("Exercise");
                });

            modelBuilder.Entity("OnlyCreateDatabase.Model.FileUpload", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Uploaded")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("OnlyCreateDatabase.Model.Grade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FileUploadId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GradeProcentage")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsRated")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("PostDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("FileUploadId");

                    b.HasIndex("UserId");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("OnlyCreateDatabase.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Surname")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("createdAt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("updatedAt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OnlyCreateDatabase.Model.Course", b =>
                {
                    b.HasOne("OnlyCreateDatabase.Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("OnlyCreateDatabase.Model.Enrollment", b =>
                {
                    b.HasOne("OnlyCreateDatabase.Model.Course", "Course")
                        .WithMany("Enrollments")
                        .HasForeignKey("CourseId");

                    b.HasOne("OnlyCreateDatabase.Model.User", "User")
                        .WithMany("Enrollments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OnlyCreateDatabase.Model.Exercise", b =>
                {
                    b.HasOne("OnlyCreateDatabase.Model.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlyCreateDatabase.Model.FileUpload", "FileUpload")
                        .WithMany()
                        .HasForeignKey("FileUploadId");

                    b.Navigation("Course");

                    b.Navigation("FileUpload");
                });

            modelBuilder.Entity("OnlyCreateDatabase.Model.Grade", b =>
                {
                    b.HasOne("OnlyCreateDatabase.Model.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlyCreateDatabase.Model.FileUpload", "FileUpload")
                        .WithMany()
                        .HasForeignKey("FileUploadId");

                    b.HasOne("OnlyCreateDatabase.Model.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("FileUpload");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OnlyCreateDatabase.Model.Course", b =>
                {
                    b.Navigation("Enrollments");
                });

            modelBuilder.Entity("OnlyCreateDatabase.Model.User", b =>
                {
                    b.Navigation("Enrollments");
                });
#pragma warning restore 612, 618
        }
    }
}
