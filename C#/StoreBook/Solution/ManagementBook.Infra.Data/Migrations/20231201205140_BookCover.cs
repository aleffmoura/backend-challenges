#nullable disable

namespace ManagementBook.Infra.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    /// <inheritdoc />
    public partial class BookCover : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookCoverUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookCoverUrl",
                table: "Books");
        }
    }
}
