using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyNewsApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewsCoulmnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NewsContext",
                table: "News",
                newName: "NewsContent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NewsContent",
                table: "News",
                newName: "NewsContext");
        }
    }
}
