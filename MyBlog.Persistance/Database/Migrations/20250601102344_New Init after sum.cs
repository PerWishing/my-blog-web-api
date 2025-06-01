using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlog.Persistance.Database.Migrations
{
    /// <inheritdoc />
    public partial class NewInitaftersum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Summarizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InputText = table.Column<string>(type: "TEXT", nullable: true),
                    OutputSummarizedText = table.Column<string>(type: "TEXT", nullable: true),
                    InputFileId = table.Column<int>(type: "INTEGER", nullable: true),
                    OutputSummarizedFileId = table.Column<int>(type: "INTEGER", nullable: true),
                    TopicText = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SummarizationType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summarizations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Summarizations");
        }
    }
}
