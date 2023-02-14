using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSMarketing.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsSystemManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystemManager",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystemManager",
                table: "AspNetUsers");
        }
    }
}
