using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookLibraryAPI.Migrations
{
    public partial class BorrowingBookIdForeignKeyAdded2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_BookID",
                table: "Borrowings",
                column: "BookID");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_Books_BookID",
                table: "Borrowings",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_Books_BookID",
                table: "Borrowings");

            migrationBuilder.DropIndex(
                name: "IX_Borrowings_BookID",
                table: "Borrowings");
        }
    }
}
