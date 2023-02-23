using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_VilleMarketing.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BillingAddresses",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingNum = table.Column<int>(type: "int", nullable: false),
                    AptNum = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetSuffix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingAddresses", x => x.AddressID);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Client_FName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Client_LName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Client_Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Client_Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BusinessID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Customer_FName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_LName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNum = table.Column<int>(type: "int", nullable: false),
                    BusinessID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    BusinessID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    AddressID = table.Column<int>(type: "int", nullable: false),
                    FacebookID = table.Column<int>(type: "int", nullable: false),
                    TikTokID = table.Column<int>(type: "int", nullable: false),
                    TwitterID = table.Column<int>(type: "int", nullable: false),
                    TwilioID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.BusinessID);
                    table.ForeignKey(
                        name: "FK_Businesses_BillingAddresses_AddressID",
                        column: x => x.AddressID,
                        principalTable: "BillingAddresses",
                        principalColumn: "AddressID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Businesses_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ClientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessCustomer",
                columns: table => new
                {
                    BusinessesBusinessID = table.Column<int>(type: "int", nullable: false),
                    CustomersCustomerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessCustomer", x => new { x.BusinessesBusinessID, x.CustomersCustomerID });
                    table.ForeignKey(
                        name: "FK_BusinessCustomer_Businesses_BusinessesBusinessID",
                        column: x => x.BusinessesBusinessID,
                        principalTable: "Businesses",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessCustomer_Customers_CustomersCustomerID",
                        column: x => x.CustomersCustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessUsageTracker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessID = table.Column<int>(type: "int", nullable: false),
                    TwilioMsgSent = table.Column<int>(type: "int", nullable: false),
                    TwitterSent = table.Column<bool>(type: "bit", nullable: false),
                    FacebookSent = table.Column<bool>(type: "bit", nullable: false),
                    TikTokSent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessUsageTracker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessUsageTracker_Businesses_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "Businesses",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacebookAccounts",
                columns: table => new
                {
                    FacebookID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Facebook_Acc_Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Facebook_Acc_Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacebookAccounts", x => x.FacebookID);
                    table.ForeignKey(
                        name: "FK_FacebookAccounts_Businesses_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "Businesses",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tikTokAccounts",
                columns: table => new
                {
                    TikTokID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TikTokOpenID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TikTokAccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tikTokAccounts", x => x.TikTokID);
                    table.ForeignKey(
                        name: "FK_tikTokAccounts_Businesses_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "Businesses",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TwilioAccounts",
                columns: table => new
                {
                    TwilioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TWilioNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwilioAccounts", x => x.TwilioID);
                    table.ForeignKey(
                        name: "FK_TwilioAccounts_Businesses_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "Businesses",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TwitterAccounts",
                columns: table => new
                {
                    TwitterID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Twit_Token_1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Twit_Token_2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Twit_Token_3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Twit_Token_4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BusinessID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterAccounts", x => x.TwitterID);
                    table.ForeignKey(
                        name: "FK_TwitterAccounts_Businesses_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "Businesses",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User_Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_Email);
                    table.ForeignKey(
                        name: "FK_Users_Businesses_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "Businesses",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCustomer_CustomersCustomerID",
                table: "BusinessCustomer",
                column: "CustomersCustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_AddressID",
                table: "Businesses",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_ClientID",
                table: "Businesses",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUsageTracker_BusinessID",
                table: "BusinessUsageTracker",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Client_Email",
                table: "Clients",
                column: "Client_Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FacebookAccounts_BusinessID",
                table: "FacebookAccounts",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_tikTokAccounts_BusinessID",
                table: "tikTokAccounts",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_TwilioAccounts_BusinessID",
                table: "TwilioAccounts",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterAccounts_BusinessID",
                table: "TwitterAccounts",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BusinessID",
                table: "Users",
                column: "BusinessID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessCustomer");

            migrationBuilder.DropTable(
                name: "BusinessUsageTracker");

            migrationBuilder.DropTable(
                name: "FacebookAccounts");

            migrationBuilder.DropTable(
                name: "tikTokAccounts");

            migrationBuilder.DropTable(
                name: "TwilioAccounts");

            migrationBuilder.DropTable(
                name: "TwitterAccounts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.DropTable(
                name: "BillingAddresses");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
