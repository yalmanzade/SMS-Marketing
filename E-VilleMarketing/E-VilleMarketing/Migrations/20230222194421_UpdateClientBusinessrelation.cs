using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_VilleMarketing.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClientBusinessrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessID",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "Business",
                table: "Businesses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_Business",
                table: "Businesses",
                column: "Business");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_Clients_Business",
                table: "Businesses",
                column: "Business",
                principalTable: "Clients",
                principalColumn: "ClientID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_Clients_Business",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_Business",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Business",
                table: "Businesses");

            migrationBuilder.AddColumn<int>(
                name: "BusinessID",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
