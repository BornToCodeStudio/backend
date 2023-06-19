using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace borntocode_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CreationDate",
                table: "Tasks",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<List<bool>>(
                name: "Languages",
                table: "Tasks",
                type: "boolean[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Languages",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Tasks");
        }
    }
}
