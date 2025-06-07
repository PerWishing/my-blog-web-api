using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlog.Persistance.Database.Migrations
{
    /// <inheritdoc />
    public partial class Addedsumcollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Summarizations_PostId",
                table: "Summarizations");

            migrationBuilder.CreateIndex(
                name: "IX_Summarizations_PostId",
                table: "Summarizations",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Summarizations_PostId",
                table: "Summarizations");

            migrationBuilder.CreateIndex(
                name: "IX_Summarizations_PostId",
                table: "Summarizations",
                column: "PostId",
                unique: true);
        }
    }
}
