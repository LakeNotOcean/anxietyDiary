﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistance;

#nullable disable

namespace Persistance.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Domain.Diaries.AcitvityDiary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DiaryUserId")
                        .HasColumnType("int");

                    b.Property<int>("Perfomance")
                        .HasColumnType("int");

                    b.Property<string>("Pleasure")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Work")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DiaryUserId");

                    b.ToTable("ActivityDiary");
                });

            modelBuilder.Entity("Domain.Diaries.EmotionsDiary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Column1")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Column2")
                        .HasColumnType("longtext");

                    b.Property<string>("Column3")
                        .HasColumnType("longtext");

                    b.Property<string>("Column4")
                        .HasColumnType("longtext");

                    b.Property<string>("Column5")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DiaryUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DiaryUserId");

                    b.ToTable("emotions_diary");
                });

            modelBuilder.Entity("Domain.Diaries.HumanBobyDiary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Day")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DiaryUserId")
                        .HasColumnType("int");

                    b.Property<string>("Emotions")
                        .HasColumnType("longtext");

                    b.Property<string>("HumanBody")
                        .HasColumnType("json");

                    b.Property<string>("Reactions")
                        .HasColumnType("longtext");

                    b.Property<string>("Thoughts")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Trigger")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DiaryUserId");

                    b.ToTable("HumanBodyDiary");
                });

            modelBuilder.Entity("Domain.Diaries.InitialDiary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Column1")
                        .HasColumnType("longtext");

                    b.Property<string>("Column2")
                        .HasColumnType("longtext");

                    b.Property<string>("Column3")
                        .HasColumnType("longtext");

                    b.Property<string>("Column4")
                        .HasColumnType("longtext");

                    b.Property<string>("Column5")
                        .HasColumnType("longtext");

                    b.Property<string>("Column6")
                        .HasColumnType("longtext");

                    b.Property<string>("Column7")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DiaryUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DiaryUserId");

                    b.ToTable("InitialDiary");
                });

            modelBuilder.Entity("Domain.Diaries.WrongRulesDiary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ChangeDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Column1")
                        .HasColumnType("longtext");

                    b.Property<string>("Column2")
                        .HasColumnType("longtext");

                    b.Property<string>("Column3")
                        .HasColumnType("longtext");

                    b.Property<string>("Column4")
                        .HasColumnType("longtext");

                    b.Property<string>("Column5")
                        .HasColumnType("longtext");

                    b.Property<string>("Column6")
                        .HasColumnType("longtext");

                    b.Property<string>("Column7")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DiaryUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DiaryUserId");

                    b.ToTable("wrong_rules_diary");
                });

            modelBuilder.Entity("Domain.DiaryExpensions.ColumnPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ColumnId")
                        .HasColumnType("int");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ColumnId")
                        .IsUnique();

                    b.ToTable("ColumnPosition");
                });

            modelBuilder.Entity("Domain.DiaryExpensions.DiaryCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Domain.DiaryExpensions.DiaryColumn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DescriptionId")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("ValueType")
                        .HasColumnType("int");

                    b.Property<bool>("isOptional")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("DiaryColumn");

                    b.HasDiscriminator<string>("Discriminator").HasValue("DiaryColumn");
                });

            modelBuilder.Entity("Domain.DiaryExpensions.DiaryDescription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("ShortName")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Descriptions");
                });

            modelBuilder.Entity("Domain.User.LastUserView", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DiaryName")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("LastViewDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValue(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

                    b.Property<int>("UserDoctorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserDoctorId");

                    b.ToTable("UsersViews");
                });

            modelBuilder.Entity("Domain.User.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("Revoked")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.Property<int?>("TokenUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TokenUserId");

                    b.ToTable("refresh_token");
                });

            modelBuilder.Entity("Domain.User.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Domain.User.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("longtext")
                        .HasDefaultValue("");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FirstName")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValue("");

                    b.Property<DateTime>("LastRecordModify")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValue(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(60)
                        .HasColumnType("varchar(60)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("SecondName")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasDefaultValue("");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("isSearching")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.User.UserDoctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<int>("PatientId")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PatientId");

                    b.ToTable("UserDoctors");
                });

            modelBuilder.Entity("Domain.User.UserRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Request")
                        .HasColumnType("int");

                    b.Property<int>("UserSourceId")
                        .HasColumnType("int");

                    b.Property<int?>("UserTargetId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserSourceId");

                    b.HasIndex("UserTargetId");

                    b.ToTable("UsersRequests");
                });

            modelBuilder.Entity("Domain.DiaryExpensions.ArbitraryColumn", b =>
                {
                    b.HasBaseType("Domain.DiaryExpensions.DiaryColumn");

                    b.Property<int?>("DiaryDescriptionId")
                        .HasColumnType("int");

                    b.HasIndex("DiaryDescriptionId");

                    b.HasDiscriminator().HasValue("ArbitraryColumn");
                });

            modelBuilder.Entity("Domain.DiaryExpensions.NonArbitraryColumn", b =>
                {
                    b.HasBaseType("Domain.DiaryExpensions.DiaryColumn");

                    b.Property<int?>("DiaryDescriptionId")
                        .HasColumnType("int")
                        .HasColumnName("NonArbitraryColumn_DiaryDescriptionId");

                    b.HasIndex("DiaryDescriptionId");

                    b.HasDiscriminator().HasValue("NonArbitraryColumn");
                });

            modelBuilder.Entity("Domain.Diaries.AcitvityDiary", b =>
                {
                    b.HasOne("Domain.User.User", "DiaryUser")
                        .WithMany()
                        .HasForeignKey("DiaryUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiaryUser");
                });

            modelBuilder.Entity("Domain.Diaries.EmotionsDiary", b =>
                {
                    b.HasOne("Domain.User.User", "DiaryUser")
                        .WithMany()
                        .HasForeignKey("DiaryUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiaryUser");
                });

            modelBuilder.Entity("Domain.Diaries.HumanBobyDiary", b =>
                {
                    b.HasOne("Domain.User.User", "DiaryUser")
                        .WithMany()
                        .HasForeignKey("DiaryUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiaryUser");
                });

            modelBuilder.Entity("Domain.Diaries.InitialDiary", b =>
                {
                    b.HasOne("Domain.User.User", "DiaryUser")
                        .WithMany()
                        .HasForeignKey("DiaryUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiaryUser");
                });

            modelBuilder.Entity("Domain.Diaries.WrongRulesDiary", b =>
                {
                    b.HasOne("Domain.User.User", "DiaryUser")
                        .WithMany()
                        .HasForeignKey("DiaryUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DiaryUser");
                });

            modelBuilder.Entity("Domain.DiaryExpensions.ColumnPosition", b =>
                {
                    b.HasOne("Domain.DiaryExpensions.DiaryColumn", "column")
                        .WithOne("Position")
                        .HasForeignKey("Domain.DiaryExpensions.ColumnPosition", "ColumnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("column");
                });

            modelBuilder.Entity("Domain.DiaryExpensions.DiaryDescription", b =>
                {
                    b.HasOne("Domain.DiaryExpensions.DiaryCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Domain.User.LastUserView", b =>
                {
                    b.HasOne("Domain.User.UserDoctor", "UserDoctor")
                        .WithMany()
                        .HasForeignKey("UserDoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserDoctor");
                });

            modelBuilder.Entity("Domain.User.RefreshToken", b =>
                {
                    b.HasOne("Domain.User.User", "TokenUser")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("TokenUserId");

                    b.Navigation("TokenUser");
                });

            modelBuilder.Entity("Domain.User.User", b =>
                {
                    b.HasOne("Domain.User.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Domain.User.UserDoctor", b =>
                {
                    b.HasOne("Domain.User.User", "Doctor")
                        .WithMany("Doctors")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.User.User", "Patient")
                        .WithMany("Patients")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("Domain.User.UserRequest", b =>
                {
                    b.HasOne("Domain.User.User", "UserSource")
                        .WithMany()
                        .HasForeignKey("UserSourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.User.User", "UserTarget")
                        .WithMany()
                        .HasForeignKey("UserTargetId");

                    b.Navigation("UserSource");

                    b.Navigation("UserTarget");
                });

            modelBuilder.Entity("Domain.DiaryExpensions.ArbitraryColumn", b =>
                {
                    b.HasOne("Domain.DiaryExpensions.DiaryDescription", null)
                        .WithMany("ArbitraryColumns")
                        .HasForeignKey("DiaryDescriptionId");
                });

            modelBuilder.Entity("Domain.DiaryExpensions.NonArbitraryColumn", b =>
                {
                    b.HasOne("Domain.DiaryExpensions.DiaryDescription", null)
                        .WithMany("NonArbitraryColumns")
                        .HasForeignKey("DiaryDescriptionId");
                });

            modelBuilder.Entity("Domain.DiaryExpensions.DiaryColumn", b =>
                {
                    b.Navigation("Position");
                });

            modelBuilder.Entity("Domain.DiaryExpensions.DiaryDescription", b =>
                {
                    b.Navigation("ArbitraryColumns");

                    b.Navigation("NonArbitraryColumns");
                });

            modelBuilder.Entity("Domain.User.User", b =>
                {
                    b.Navigation("Doctors");

                    b.Navigation("Patients");

                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
