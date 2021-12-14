using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Seventh.DGuard.Database.Migrations
{
    public partial class AddVideoDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddDate",
                table: "Video",
                nullable: false,
                defaultValueSql: "getdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddDate",
                table: "Video");
        }
    }
}
