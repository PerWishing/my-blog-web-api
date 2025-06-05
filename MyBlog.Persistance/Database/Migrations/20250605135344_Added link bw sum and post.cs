using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlog.Persistance.Database.Migrations
{
    /// <inheritdoc />
    public partial class Addedlinkbwsumandpost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Summarizations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Summarizations_PostId",
                table: "Summarizations",
                column: "PostId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Summarizations_Posts_PostId",
                table: "Summarizations",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Summarizations_Posts_PostId",
                table: "Summarizations");

            migrationBuilder.DropIndex(
                name: "IX_Summarizations_PostId",
                table: "Summarizations");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Summarizations");
        }
    }
}
