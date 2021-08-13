using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreIdentity.Migrations
{
    public partial class AddNationToAppRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nation",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nation",
                table: "AspNetRoles");
        }
    }
}
