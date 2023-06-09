using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyNewsApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsDeletedColFromNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "News");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "News",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
