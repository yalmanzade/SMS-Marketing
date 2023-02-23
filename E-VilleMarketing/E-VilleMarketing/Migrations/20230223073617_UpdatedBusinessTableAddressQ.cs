using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_VilleMarketing.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBusinessTableAddressQ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_BillingAddresses_BillingAddressAddressID",
                table: "Businesses");

            migrationBuilder.AlterColumn<int>(
                name: "BillingAddressAddressID",
                table: "Businesses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_BillingAddresses_BillingAddressAddressID",
                table: "Businesses",
                column: "BillingAddressAddressID",
                principalTable: "BillingAddresses",
                principalColumn: "AddressID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_BillingAddresses_BillingAddressAddressID",
                table: "Businesses");

            migrationBuilder.AlterColumn<int>(
                name: "BillingAddressAddressID",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_BillingAddresses_BillingAddressAddressID",
                table: "Businesses",
                column: "BillingAddressAddressID",
                principalTable: "BillingAddresses",
                principalColumn: "AddressID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
