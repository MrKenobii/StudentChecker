﻿// <auto-generated />
using System;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Backend.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("Backend.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("Backend.Models.College", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("EmailExtension")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FoundationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Colleges");
                });

            modelBuilder.Entity("Backend.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("CompanyKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CompanyType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FoundationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Backend.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Backend.Models.DeliveredMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DeliveredTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("RecruiterId")
                        .HasColumnType("int");

                    b.Property<int?>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RecruiterId");

                    b.HasIndex("StudentId");

                    b.ToTable("DeliveredMessages");
                });

            modelBuilder.Entity("Backend.Models.Recruiter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("HireDate")
                        .HasColumnType("datetime(6)");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<bool>("IsActivated")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.Property<string>("VerifyToken")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Recruiters");
                });

            modelBuilder.Entity("Backend.Models.RecruiterCompany", b =>
                {
                    b.Property<int>("RecruiterId")
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.HasKey("RecruiterId", "CompanyId");

                    b.HasIndex("CompanyId");

                    b.ToTable("RecruiterCompanies");
                });

            modelBuilder.Entity("Backend.Models.SendMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("RecruiterId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SendTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RecruiterId");

                    b.HasIndex("StudentId");

                    b.ToTable("SendMessages");
                });

            modelBuilder.Entity("Backend.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<int?>("CityId")
                        .HasColumnType("int");

                    b.Property<int?>("CollegeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Department")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("EnrollDate")
                        .HasColumnType("datetime(6)");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<bool>("IsActivated")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Languages")
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.Property<string>("Skills")
                        .HasColumnType("longtext");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.Property<string>("VerifyToken")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CollegeId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Backend.Models.StudentCourse", b =>
                {
                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.HasKey("StudentId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("StudentCourses");
                });

            modelBuilder.Entity("Backend.Models.College", b =>
                {
                    b.HasOne("Backend.Models.City", "City")
                        .WithMany("Colleges")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("Backend.Models.Company", b =>
                {
                    b.HasOne("Backend.Models.City", "City")
                        .WithMany("Companies")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("Backend.Models.DeliveredMessage", b =>
                {
                    b.HasOne("Backend.Models.Recruiter", "Recruiter")
                        .WithMany("DeliveredMessages")
                        .HasForeignKey("RecruiterId");

                    b.HasOne("Backend.Models.Student", "Student")
                        .WithMany("DeliveredMessages")
                        .HasForeignKey("StudentId");

                    b.Navigation("Recruiter");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Backend.Models.RecruiterCompany", b =>
                {
                    b.HasOne("Backend.Models.Company", "Company")
                        .WithMany("RecruiterCompanies")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.Recruiter", "Recruiter")
                        .WithMany("RecruiterCompanies")
                        .HasForeignKey("RecruiterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Recruiter");
                });

            modelBuilder.Entity("Backend.Models.SendMessage", b =>
                {
                    b.HasOne("Backend.Models.Recruiter", "Recruiter")
                        .WithMany("SendMessages")
                        .HasForeignKey("RecruiterId");

                    b.HasOne("Backend.Models.Student", "Student")
                        .WithMany("SendMessages")
                        .HasForeignKey("StudentId");

                    b.Navigation("Recruiter");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Backend.Models.Student", b =>
                {
                    b.HasOne("Backend.Models.City", "City")
                        .WithMany("Students")
                        .HasForeignKey("CityId");

                    b.HasOne("Backend.Models.College", "College")
                        .WithMany("Students")
                        .HasForeignKey("CollegeId");

                    b.Navigation("City");

                    b.Navigation("College");
                });

            modelBuilder.Entity("Backend.Models.StudentCourse", b =>
                {
                    b.HasOne("Backend.Models.Course", "Course")
                        .WithMany("StudentCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.Student", "Student")
                        .WithMany("StudentCourses")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Backend.Models.City", b =>
                {
                    b.Navigation("Colleges");

                    b.Navigation("Companies");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("Backend.Models.College", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("Backend.Models.Company", b =>
                {
                    b.Navigation("RecruiterCompanies");
                });

            modelBuilder.Entity("Backend.Models.Course", b =>
                {
                    b.Navigation("StudentCourses");
                });

            modelBuilder.Entity("Backend.Models.Recruiter", b =>
                {
                    b.Navigation("DeliveredMessages");

                    b.Navigation("RecruiterCompanies");

                    b.Navigation("SendMessages");
                });

            modelBuilder.Entity("Backend.Models.Student", b =>
                {
                    b.Navigation("DeliveredMessages");

                    b.Navigation("SendMessages");

                    b.Navigation("StudentCourses");
                });
#pragma warning restore 612, 618
        }
    }
}
