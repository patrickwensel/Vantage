using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vantage.Data.Migrations
{
    public partial class MadeGroupNullableFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attempts_Drivers_DriverID",
                table: "Attempts");

            migrationBuilder.DropForeignKey(
                name: "FK_Infractions_Attempts_AttemptID",
                table: "Infractions");

            migrationBuilder.AlterColumn<int>(
                name: "GroupID",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "Drivers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCompleted",
                table: "Attempts",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_ProductID",
                table: "Drivers",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attempts_Drivers_DriverID",
                table: "Attempts",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "DriverID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Products_ProductID",
                table: "Drivers",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Infractions_Attempts_AttemptID",
                table: "Infractions",
                column: "AttemptID",
                principalTable: "Attempts",
                principalColumn: "AttemptID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attempts_Drivers_DriverID",
                table: "Attempts");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Products_ProductID",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Infractions_Attempts_AttemptID",
                table: "Infractions");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_ProductID",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "Drivers");

            migrationBuilder.AlterColumn<int>(
                name: "GroupID",
                table: "Drivers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCompleted",
                table: "Attempts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attempts_Drivers_DriverID",
                table: "Attempts",
                column: "DriverID",
                principalTable: "Drivers",
                principalColumn: "DriverID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Infractions_Attempts_AttemptID",
                table: "Infractions",
                column: "AttemptID",
                principalTable: "Attempts",
                principalColumn: "AttemptID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
