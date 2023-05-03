﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using servis.Models;

#nullable disable

namespace servis.Migrations
{
    [DbContext(typeof(PsychologistDBContext))]
    [Migration("20230501195708_MigrateDB21")]
    partial class MigrateDB21
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("servis.Models.Client", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("servis.Models.GetSession", b =>
                {
                    b.Property<int>("Session_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Session_ID"), 1L, 1);

                    b.Property<DateTime>("Date_Session")
                        .HasColumnType("datetime2");

                    b.Property<int>("Format_Session")
                        .HasColumnType("int");

                    b.Property<int>("Psychologist_objId")
                        .HasColumnType("int");

                    b.HasKey("Session_ID");

                    b.HasIndex("Psychologist_objId");

                    b.ToTable("GetSession");
                });

            modelBuilder.Entity("servis.Models.Methods", b =>
                {
                    b.Property<int>("Methods_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Methods_ID"), 1L, 1);

                    b.Property<string>("Methods_Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Methods_ID");

                    b.ToTable("Methods");
                });

            modelBuilder.Entity("servis.Models.Psychologist", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Info")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Methods_objId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Specialization_objId")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Methods_objId");

                    b.HasIndex("Specialization_objId");

                    b.ToTable("Psychologist");
                });

            modelBuilder.Entity("servis.Models.Specialization", b =>
                {
                    b.Property<int>("Special_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Special_ID"), 1L, 1);

                    b.Property<string>("Special_Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Special_ID");

                    b.ToTable("Specialization");
                });

            modelBuilder.Entity("servis.Models.GetSession", b =>
                {
                    b.HasOne("servis.Models.Psychologist", "Psychologist_obj")
                        .WithMany()
                        .HasForeignKey("Psychologist_objId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Psychologist_obj");
                });

            modelBuilder.Entity("servis.Models.Psychologist", b =>
                {
                    b.HasOne("servis.Models.Methods", "Methods_obj")
                        .WithMany()
                        .HasForeignKey("Methods_objId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("servis.Models.Specialization", "Specialization_obj")
                        .WithMany()
                        .HasForeignKey("Specialization_objId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Methods_obj");

                    b.Navigation("Specialization_obj");
                });
#pragma warning restore 612, 618
        }
    }
}
