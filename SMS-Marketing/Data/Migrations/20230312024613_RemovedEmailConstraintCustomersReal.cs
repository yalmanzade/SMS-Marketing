using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMSMarketing.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedEmailConstraintCustomersReal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "Email",
            table: "Customers");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
