using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPJWTPractice.Migrations.AppDb
{
    public partial class secmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "Modified",
                table: "Users",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "Modified",
                table: "RefreshTokens",
                newName: "DateCreated");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "RefreshTokens",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Users",
                newName: "Modified");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "RefreshTokens",
                newName: "Modified");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "RefreshTokens",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
