using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyNewsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverImgPathPropertyToNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImgPath",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImgPath",
                table: "News");
        }
    }
}
