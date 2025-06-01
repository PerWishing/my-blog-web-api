using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlog.Persistance.Database.Migrations
{
    /// <inheritdoc />
    public partial class Addedlinkbwsumanduser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Summarizations");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Summarizations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Summarizations_AuthorId",
                table: "Summarizations",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Summarizations_UserProfiles_AuthorId",
                table: "Summarizations",
                column: "AuthorId",
                principalTable: "UserProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Summarizations_UserProfiles_AuthorId",
                table: "Summarizations");

            migrationBuilder.DropIndex(
                name: "IX_Summarizations_AuthorId",
                table: "Summarizations");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Summarizations");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Summarizations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
