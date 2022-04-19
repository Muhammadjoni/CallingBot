﻿// <auto-generated />
using CallingBotSample.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CallingBotSample.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("CallingBotSample.Model.CallDetails", b =>
                {
                    b.Property<string>("CallId")
                        .HasColumnType("text");

                    b.Property<string>("ParticipantId")
                        .HasColumnType("text");

                    b.Property<string>("ParticipantName")
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .HasColumnType("text");

                    b.ToTable("CallDetails");
                });

            modelBuilder.Entity("CallingBotSample.Model.ParticipantDetails", b =>
                {
                    b.Property<string>("EmailId")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("TenantId")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.ToTable("ParticipantDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
