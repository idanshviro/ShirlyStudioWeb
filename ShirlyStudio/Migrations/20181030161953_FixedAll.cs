using Microsoft.EntityFrameworkCore.Migrations;

namespace ShirlyStudio.Migrations
{
    public partial class FixedAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Teacher",
                newName: "TeacherName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Customer",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Customer",
                newName: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeacherName",
                table: "Teacher",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "Customer",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Customer",
                newName: "Id");
        }
    }
}
