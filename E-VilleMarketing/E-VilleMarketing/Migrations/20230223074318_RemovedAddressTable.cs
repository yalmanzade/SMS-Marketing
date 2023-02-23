using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_VilleMarketing.Migrations
{
    /// <inheritdoc />
    public partial class RemovedAddressTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_BillingAddresses_BillingAddressAddressID",
                table: "Businesses");

            migrationBuilder.DropTable(
                name: "BillingAddresses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_BillingAddressAddressID",
                table: "Businesses");

            migrationBuilder.RenameColumn(
                name: "BillingAddressAddressID",
                table: "Businesses",
                newName: "AptNum");

            migrationBuilder.AddColumn<int>(
                name: "BuildingNum",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StreetName",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ZipCode",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingNum",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "StreetName",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Businesses");

            migrationBuilder.RenameColumn(
                name: "AptNum",
                table: "Businesses",
                newName: "BillingAddressAddressID");

            migrationBuilder.CreateTable(
                name: "BillingAddresses",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AptNum = table.Column<int>(type: "int", nullable: false),
                    BuildingNum = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetSuffix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingAddresses", x => x.AddressID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_BillingAddressAddressID",
                table: "Businesses",
                column: "BillingAddressAddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_BillingAddresses_BillingAddressAddressID",
                table: "Businesses",
                column: "BillingAddressAddressID",
                principalTable: "BillingAddresses",
                principalColumn: "AddressID");
        }
    }
}
