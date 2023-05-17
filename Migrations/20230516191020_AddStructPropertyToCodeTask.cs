using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace borntocode_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddStructPropertyToCodeTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Struct",
                table: "Tasks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Struct",
                table: "Tasks");
        }
    }
}
