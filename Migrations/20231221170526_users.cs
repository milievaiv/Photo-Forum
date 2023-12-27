using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoForum.Migrations
{
    public partial class users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_BaseUsers_Id",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "BaseUsers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "BaseUsers");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "RegularUsers");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "BaseUsers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "BaseUsers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RegularUsers",
                table: "RegularUsers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_RegularUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "RegularUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_RegularUsers_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "RegularUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegularUsers_BaseUsers_Id",
                table: "RegularUsers",
                column: "Id",
                principalTable: "BaseUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_RegularUsers_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_RegularUsers_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_RegularUsers_BaseUsers_Id",
                table: "RegularUsers");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RegularUsers",
                table: "RegularUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "BaseUsers");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "BaseUsers");

            migrationBuilder.RenameTable(
                name: "RegularUsers",
                newName: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "BaseUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "BaseUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_BaseUsers_Id",
                table: "Users",
                column: "Id",
                principalTable: "BaseUsers",
                principalColumn: "Id");
        }
    }
}
