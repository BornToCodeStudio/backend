using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace borntocode_backend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLanguagesFromTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Languages",
                table: "Tasks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<bool>>(
                name: "Languages",
                table: "Tasks",
                type: "boolean[]",
                nullable: true);
        }
    }
}
