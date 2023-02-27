using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSMarketing.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyAppSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SettingGroup",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SettingGroup",
                table: "AppSettings");
        }
    }
}
