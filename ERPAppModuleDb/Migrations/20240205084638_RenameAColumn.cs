using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPAppModuleDb.Migrations
{
    public partial class RenameAColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Trucks",
                newName: "UpdatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Trucks",
                newName: "CreatedAt");
        }
    }
}
