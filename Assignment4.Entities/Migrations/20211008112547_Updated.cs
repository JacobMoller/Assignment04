using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Assignment4.Entities.Migrations
{
    public partial class Updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagTask_Tasks_taskstaskId",
                table: "TagTask");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_assignedTouserId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "assignedTouserId",
                table: "Tasks",
                newName: "assignedToid");

            migrationBuilder.RenameColumn(
                name: "taskId",
                table: "Tasks",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_assignedTouserId",
                table: "Tasks",
                newName: "IX_Tasks_assignedToid");

            migrationBuilder.RenameColumn(
                name: "taskstaskId",
                table: "TagTask",
                newName: "tasksid");

            migrationBuilder.RenameIndex(
                name: "IX_TagTask_taskstaskId",
                table: "TagTask",
                newName: "IX_TagTask_tasksid");

            migrationBuilder.AddColumn<DateTime>(
                name: "created",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "stateUpdated",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_TagTask_Tasks_tasksid",
                table: "TagTask",
                column: "tasksid",
                principalTable: "Tasks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_assignedToid",
                table: "Tasks",
                column: "assignedToid",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagTask_Tasks_tasksid",
                table: "TagTask");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_assignedToid",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "created",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "stateUpdated",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "assignedToid",
                table: "Tasks",
                newName: "assignedTouserId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Tasks",
                newName: "taskId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_assignedToid",
                table: "Tasks",
                newName: "IX_Tasks_assignedTouserId");

            migrationBuilder.RenameColumn(
                name: "tasksid",
                table: "TagTask",
                newName: "taskstaskId");

            migrationBuilder.RenameIndex(
                name: "IX_TagTask_tasksid",
                table: "TagTask",
                newName: "IX_TagTask_taskstaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagTask_Tasks_taskstaskId",
                table: "TagTask",
                column: "taskstaskId",
                principalTable: "Tasks",
                principalColumn: "taskId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_assignedTouserId",
                table: "Tasks",
                column: "assignedTouserId",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
