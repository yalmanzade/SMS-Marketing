using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_VilleMarketing.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClientBusinessRelationReal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
