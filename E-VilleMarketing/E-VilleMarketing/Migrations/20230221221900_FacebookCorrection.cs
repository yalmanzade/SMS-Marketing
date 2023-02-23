using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_VilleMarketing.Migrations
{
    /// <inheritdoc />
    public partial class FacebookCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Facebook_Acc_Link",
                table: "FacebookAccounts",
                newName: "FacebookAppId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_User_Email",
                table: "Users",
                column: "User_Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_User_Email",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "FacebookAppId",
                table: "FacebookAccounts",
                newName: "Facebook_Acc_Link");
        }
    }
}
