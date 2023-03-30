using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSMarketing.Data.Migrations
{
    /// <inheritdoc />
    public partial class ContextCleanUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authorization");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authorization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsCustomerManagment = table.Column<bool>(type: "bit", nullable: false),
                    IsInsight = table.Column<bool>(type: "bit", nullable: false),
                    IsPost = table.Column<bool>(type: "bit", nullable: false),
                    IsUserManagement = table.Column<bool>(type: "bit", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorization", x => x.Id);
                });
        }
    }
}
