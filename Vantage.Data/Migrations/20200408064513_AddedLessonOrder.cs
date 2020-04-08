using Microsoft.EntityFrameworkCore.Migrations;

namespace Vantage.Data.Migrations
{
    public partial class AddedLessonOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LessonOrder",
                table: "Lessons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Lessons",
                keyColumn: "LessonID",
                keyValue: 1,
                column: "LessonOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Lessons",
                keyColumn: "LessonID",
                keyValue: 2,
                column: "LessonOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Lessons",
                keyColumn: "LessonID",
                keyValue: 3,
                column: "LessonOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Lessons",
                keyColumn: "LessonID",
                keyValue: 4,
                column: "LessonOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Lessons",
                keyColumn: "LessonID",
                keyValue: 5,
                column: "LessonOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Lessons",
                keyColumn: "LessonID",
                keyValue: 6,
                column: "LessonOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Lessons",
                keyColumn: "LessonID",
                keyValue: 7,
                column: "LessonOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Lessons",
                keyColumn: "LessonID",
                keyValue: 8,
                column: "LessonOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Lessons",
                keyColumn: "LessonID",
                keyValue: 9,
                column: "LessonOrder",
                value: 9);

            migrationBuilder.UpdateData(
                table: "Lessons",
                keyColumn: "LessonID",
                keyValue: 10,
                column: "LessonOrder",
                value: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LessonOrder",
                table: "Lessons");
        }
    }
}
