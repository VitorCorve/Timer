// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimerContext.Models.Context;

#nullable disable

namespace TimerContext.Migrations
{
    [DbContext(typeof(TimerDbContext))]
    [Migration("20220822113330_M3LastStatUpdate")]
    partial class M3LastStatUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TimerContext.Models.Statistics", b =>
                {
                    b.Property<int>("StatisticsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StatisticsId"), 1L, 1);

                    b.Property<int>("LastInitDay")
                        .HasColumnType("int");

                    b.Property<int>("LastInitMonth")
                        .HasColumnType("int");

                    b.Property<int>("LastInitWeekNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("SecondsMonthCount")
                        .HasColumnType("int");

                    b.Property<int>("SecondsTodayCount")
                        .HasColumnType("int");

                    b.Property<int>("SecondsTotalCount")
                        .HasColumnType("int");

                    b.Property<int>("SecondsWeekCount")
                        .HasColumnType("int");

                    b.HasKey("StatisticsId");

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("TimerContext.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<DateTime>("DateRegister")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastSeen")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("StatisticsId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserIdentifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("UserPic")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("UserStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
