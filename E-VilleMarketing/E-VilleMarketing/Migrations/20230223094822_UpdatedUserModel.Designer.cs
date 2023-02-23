﻿// <auto-generated />
using System;
using E_VilleMarketing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace E_VilleMarketing.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230223094822_UpdatedUserModel")]
    partial class UpdatedUserModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BusinessCustomer", b =>
                {
                    b.Property<int>("BusinessesBusinessID")
                        .HasColumnType("int");

                    b.Property<int>("CustomersCustomerID")
                        .HasColumnType("int");

                    b.HasKey("BusinessesBusinessID", "CustomersCustomerID");

                    b.HasIndex("CustomersCustomerID");

                    b.ToTable("BusinessCustomer");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.Account", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("Logins");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.Business", b =>
                {
                    b.Property<int>("BusinessID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BusinessID"));

                    b.Property<int?>("AptNum")
                        .HasColumnType("int");

                    b.Property<int>("BuildingNum")
                        .HasColumnType("int");

                    b.Property<string>("BusinessName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClientID")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ZipCode")
                        .HasColumnType("int");

                    b.HasKey("BusinessID");

                    b.HasIndex("ClientID");

                    b.ToTable("Businesses");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.Client", b =>
                {
                    b.Property<int>("ClientID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClientID"));

                    b.Property<string>("Client_Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Client_FName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Client_LName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Client_Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClientID");

                    b.HasIndex("Client_Email")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerID"));

                    b.Property<int>("BusinessID")
                        .HasColumnType("int");

                    b.Property<string>("Customer_FName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Customer_LName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PhoneNum")
                        .HasColumnType("int");

                    b.HasKey("CustomerID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.Facebook", b =>
                {
                    b.Property<int>("FacebookID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FacebookID"));

                    b.Property<int>("BusinessID")
                        .HasColumnType("int");

                    b.Property<string>("FacebookAppId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Facebook_Acc_Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FacebookID");

                    b.HasIndex("BusinessID");

                    b.ToTable("FacebookAccounts");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.TikTok", b =>
                {
                    b.Property<int>("TikTokID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TikTokID"));

                    b.Property<int>("BusinessID")
                        .HasColumnType("int");

                    b.Property<string>("TikTokAccessToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TikTokOpenID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TikTokID");

                    b.HasIndex("BusinessID");

                    b.ToTable("tikTokAccounts");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.Twilio", b =>
                {
                    b.Property<int>("TwilioID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TwilioID"));

                    b.Property<int>("BusinessID")
                        .HasColumnType("int");

                    b.Property<string>("TWilioNum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TwilioID");

                    b.HasIndex("BusinessID");

                    b.ToTable("TwilioAccounts");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.Twitter", b =>
                {
                    b.Property<int>("TwitterID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TwitterID"));

                    b.Property<int>("BusinessID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Twit_Token_1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Twit_Token_2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Twit_Token_3")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Twit_Token_4")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TwitterID");

                    b.HasIndex("BusinessID");

                    b.ToTable("TwitterAccounts");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.UsageTracker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BusinessID")
                        .HasColumnType("int");

                    b.Property<bool>("FacebookSent")
                        .HasColumnType("bit");

                    b.Property<bool>("TikTokSent")
                        .HasColumnType("bit");

                    b.Property<int>("TwilioMsgSent")
                        .HasColumnType("int");

                    b.Property<bool>("TwitterSent")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("BusinessID");

                    b.ToTable("BusinessUsageTracker");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.User", b =>
                {
                    b.Property<string>("User_Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("BusinessID")
                        .HasColumnType("int");

                    b.Property<string>("User_Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("User_Email");

                    b.HasIndex("BusinessID");

                    b.HasIndex("User_Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BusinessCustomer", b =>
                {
                    b.HasOne("E_VilleMarketing.Models.Business", null)
                        .WithMany()
                        .HasForeignKey("BusinessesBusinessID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("E_VilleMarketing.Models.Customer", null)
                        .WithMany()
                        .HasForeignKey("CustomersCustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("E_VilleMarketing.Models.Business", b =>
                {
                    b.HasOne("E_VilleMarketing.Models.Client", "Client")
                        .WithMany("Businesses")
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.Facebook", b =>
                {
                    b.HasOne("E_VilleMarketing.Models.Business", "Business")
                        .WithMany("FacebookAccounts")
                        .HasForeignKey("BusinessID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.TikTok", b =>
                {
                    b.HasOne("E_VilleMarketing.Models.Business", "Business")
                        .WithMany("TikTokAccounts")
                        .HasForeignKey("BusinessID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.Twilio", b =>
                {
                    b.HasOne("E_VilleMarketing.Models.Business", "Business")
                        .WithMany("TwilioNumbers")
                        .HasForeignKey("BusinessID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.Twitter", b =>
                {
                    b.HasOne("E_VilleMarketing.Models.Business", "Business")
                        .WithMany("TwitterAccounts")
                        .HasForeignKey("BusinessID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.UsageTracker", b =>
                {
                    b.HasOne("E_VilleMarketing.Models.Business", "Business")
                        .WithMany()
                        .HasForeignKey("BusinessID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.User", b =>
                {
                    b.HasOne("E_VilleMarketing.Models.Business", "Business")
                        .WithMany("Users")
                        .HasForeignKey("BusinessID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.Business", b =>
                {
                    b.Navigation("FacebookAccounts");

                    b.Navigation("TikTokAccounts");

                    b.Navigation("TwilioNumbers");

                    b.Navigation("TwitterAccounts");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("E_VilleMarketing.Models.Client", b =>
                {
                    b.Navigation("Businesses");
                });
#pragma warning restore 612, 618
        }
    }
}
