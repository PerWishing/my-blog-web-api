using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlog.Persistance.Database.Migrations
{
    /// <inheritdoc />
    public partial class Editedsumentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputFileId",
                table: "Summarizations");

            migrationBuilder.DropColumn(
                name: "OutputSummarizedFileId",
                table: "Summarizations");

            migrationBuilder.AddColumn<string>(
                name: "InputFilePath",
                table: "Summarizations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutputSummarizedFilePath",
                table: "Summarizations",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputFilePath",
                table: "Summarizations");

            migrationBuilder.DropColumn(
                name: "OutputSummarizedFilePath",
                table: "Summarizations");

            migrationBuilder.AddColumn<int>(
                name: "InputFileId",
                table: "Summarizations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OutputSummarizedFileId",
                table: "Summarizations",
                type: "INTEGER",
                nullable: true);
        }
    }
}
