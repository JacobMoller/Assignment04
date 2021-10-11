using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment4.Entities.Migrations
{
    public partial class Update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagTask_Tags_tagstagId",
                table: "TagTask");

            migrationBuilder.RenameColumn(
                name: "tagstagId",
                table: "TagTask",
                newName: "tagsid");

            migrationBuilder.RenameColumn(
                name: "tagId",
                table: "Tags",
                newName: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TagTask_Tags_tagsid",
                table: "TagTask",
                column: "tagsid",
                principalTable: "Tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagTask_Tags_tagsid",
                table: "TagTask");

            migrationBuilder.RenameColumn(
                name: "tagsid",
                table: "TagTask",
                newName: "tagstagId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Tags",
                newName: "tagId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagTask_Tags_tagstagId",
                table: "TagTask",
                column: "tagstagId",
                principalTable: "Tags",
                principalColumn: "tagId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
