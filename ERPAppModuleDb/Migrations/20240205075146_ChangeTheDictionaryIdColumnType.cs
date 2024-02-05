using System;
using ERPAppModuleCommon;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERPAppModuleDb.Migrations
{
    public partial class ChangeTheDictionaryIdColumnType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM TruckStatuses");

            migrationBuilder.DropCheckConstraint("FK_Trucks_TruckStatuses_StatusId", "Trucks");
            
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TruckStatuses");
            
            migrationBuilder.DropPrimaryKey(
                name: "PK_TruckStatuses",
                table: "TruckStatuses");

            migrationBuilder.DropColumn("Id", "TruckStatuses");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "TruckStatuses",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "StatusId",
                table: "Trucks",
                type: "nvarchar(256)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
            
            migrationBuilder.AddPrimaryKey(
                name: "PK_TruckStatuses",
                table: "TruckStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey("FK_Trucks_TruckStatuses_StatusId", "Trucks", "StatusId", "TruckStatuses",
                principalColumn: "Id", onDelete: ReferentialAction.Restrict);            

            migrationBuilder.Sql(
                $"INSERT INTO TruckStatuses (Id, Name) VALUES ('{nameof(Constants.LoadingStatus)}', '{Constants.LoadingStatus}'), " +
                $"('{nameof(Constants.ReturningStatus)}', '{Constants.ReturningStatus}')," +
                $"('{nameof(Constants.AtJobStatus)}', '{Constants.AtJobStatus}')," +
                $"('{nameof(Constants.ToJobStatus)}', '{Constants.ToJobStatus}')," +
                $"('{nameof(Constants.OutOfServiceStatus)}', '{Constants.OutOfServiceStatus}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TruckStatuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TruckStatuses",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Trucks",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)");

            migrationBuilder.Sql(
                $"DELETE FROM TruckStatuses WHERE Id IN ({Constants.LoadingStatus}, {Constants.ReturningStatus}, {Constants.AtJobStatus}, {Constants.ToJobStatus}, {Constants.OutOfServiceStatus})");
        }
    }
}
