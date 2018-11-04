using Microsoft.EntityFrameworkCore.Migrations;

namespace ShirlyStudio.Migrations
{
    public partial class UpdateTeacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Teacher",
                newName: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Teacher",
                newName: "Id");
        }
    }
}
