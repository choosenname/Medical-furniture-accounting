﻿// <auto-generated />
using System;
using MedicalFurnitureAccounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MedicalFurnitureAccounting.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    partial class ApplicationDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0-preview.2.24128.4");

            modelBuilder.Entity("MedicalFurnitureAccounting.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("MaxWeight")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MedicalFurnitureAccounting.Models.Material", b =>
                {
                    b.Property<int>("MaterialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("MaxWeight")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Price")
                        .HasColumnType("INTEGER");

                    b.HasKey("MaterialId");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("MedicalFurnitureAccounting.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaterialId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MaxWeight")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Room")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SupplyId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("MaterialId");

                    b.HasIndex("SupplyId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MedicalFurnitureAccounting.Models.Storekeeper", b =>
                {
                    b.Property<int>("StorekeeperId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("MaxWeight")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("StorekeeperId");

                    b.ToTable("Storekeepers");
                });

            modelBuilder.Entity("MedicalFurnitureAccounting.Models.Supplier", b =>
                {
                    b.Property<int>("SupplierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("MaxWeight")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("SupplierId");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("MedicalFurnitureAccounting.Models.Supply", b =>
                {
                    b.Property<int>("SupplyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("SupplierId")
                        .HasColumnType("INTEGER");

                    b.HasKey("SupplyId");

                    b.HasIndex("SupplierId")
                        .IsUnique();

                    b.ToTable("Supplies");
                });

            modelBuilder.Entity("MedicalFurnitureAccounting.Models.Product", b =>
                {
                    b.HasOne("MedicalFurnitureAccounting.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicalFurnitureAccounting.Models.Material", "Material")
                        .WithMany("Products")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicalFurnitureAccounting.Models.Supply", "Suppply")
                        .WithMany("Products")
                        .HasForeignKey("SupplyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Material");

                    b.Navigation("Suppply");
                });

            modelBuilder.Entity("MedicalFurnitureAccounting.Models.Supply", b =>
                {
                    b.HasOne("MedicalFurnitureAccounting.Models.Supplier", "Supplier")
                        .WithOne("Supply")
                        .HasForeignKey("MedicalFurnitureAccounting.Models.Supply", "SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("MedicalFurnitureAccounting.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("MedicalFurnitureAccounting.Models.Material", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("MedicalFurnitureAccounting.Models.Supplier", b =>
                {
                    b.Navigation("Supply")
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalFurnitureAccounting.Models.Supply", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
