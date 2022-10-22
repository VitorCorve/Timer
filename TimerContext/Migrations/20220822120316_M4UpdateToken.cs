using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimerContext.Migrations
{
    public partial class M4UpdateToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LastUpdateToken",
                table: "Statistics",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdateToken",
                table: "Statistics");
        }
    }
}
