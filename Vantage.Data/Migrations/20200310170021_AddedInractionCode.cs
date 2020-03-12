using Microsoft.EntityFrameworkCore.Migrations;

namespace Vantage.Data.Migrations
{
    public partial class AddedInractionCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InfranctionCode",
                table: "Infractions",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleID",
                keyValue: 1,
                column: "Name",
                value: "Instructor");

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserRoleID", "RoleID", "UserID" },
                values: new object[] { 2, 2, 1 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "FirstName", "LastName", "Password", "UserName" },
                values: new object[] { 2, "John", "Smith", "3FBFEB0EE307127BBD4EF7DA33F7B57A9FF3C7357DA182C5BFCCC2A4F599C6F9", "JSmith" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserRoleID", "RoleID", "UserID" },
                values: new object[] { 3, 2, 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "UserRoleID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "UserRoleID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "InfranctionCode",
                table: "Infractions");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleID",
                keyValue: 1,
                column: "Name",
                value: "Facilitator");
        }
    }
}
