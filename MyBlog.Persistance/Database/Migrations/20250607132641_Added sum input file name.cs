using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlog.Persistance.Database.Migrations
{
    /// <inheritdoc />
    public partial class Addedsuminputfilename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InputFileName",
                table: "Summarizations",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputFileName",
                table: "Summarizations");
        }
    }
}
