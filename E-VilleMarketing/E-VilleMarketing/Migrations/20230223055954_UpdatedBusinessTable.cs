using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_VilleMarketing.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBusinessTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_BillingAddresses_AddressID",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_AddressID",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "AddressID",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "FacebookID",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "TikTokID",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "TwilioID",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "TwitterID",
                table: "Businesses");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Businesses",
                newName: "BillingAddressAddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_BillingAddressAddressID",
                table: "Businesses",
                column: "BillingAddressAddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_BillingAddresses_BillingAddressAddressID",
                table: "Businesses",
                column: "BillingAddressAddressID",
                principalTable: "BillingAddresses",
                principalColumn: "AddressID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_BillingAddresses_BillingAddressAddressID",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_BillingAddressAddressID",
                table: "Businesses");

            migrationBuilder.RenameColumn(
                name: "BillingAddressAddressID",
                table: "Businesses",
                newName: "UserID");

            migrationBuilder.AddColumn<int>(
                name: "AddressID",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerID",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FacebookID",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TikTokID",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TwilioID",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TwitterID",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_AddressID",
                table: "Businesses",
                column: "AddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_BillingAddresses_AddressID",
                table: "Businesses",
                column: "AddressID",
                principalTable: "BillingAddresses",
                principalColumn: "AddressID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
