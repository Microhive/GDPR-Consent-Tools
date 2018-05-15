using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace IdentityTest.Data.Migrations
{
    public partial class ConsentApplicationUserIdchangedtypefromGuidtostring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consents_AspNetUsers_ApplicationUserId",
                table: "Consents");

            migrationBuilder.DropForeignKey(
                name: "FK_Consents_Purposes_PurposeTitle",
                table: "Consents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Purposes",
                table: "Purposes");

            migrationBuilder.DropIndex(
                name: "IX_Consents_PurposeTitle",
                table: "Consents");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Consents");

            migrationBuilder.DropColumn(
                name: "PurposeTitle",
                table: "Consents");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Purposes",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Purposes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Purposes",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Consents",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PurposeId",
                table: "Consents",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Purposes",
                table: "Purposes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Consents_PurposeId",
                table: "Consents",
                column: "PurposeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consents_AspNetUsers_ApplicationUserId",
                table: "Consents",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consents_Purposes_PurposeId",
                table: "Consents",
                column: "PurposeId",
                principalTable: "Purposes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consents_AspNetUsers_ApplicationUserId",
                table: "Consents");

            migrationBuilder.DropForeignKey(
                name: "FK_Consents_Purposes_PurposeId",
                table: "Consents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Purposes",
                table: "Purposes");

            migrationBuilder.DropIndex(
                name: "IX_Consents_PurposeId",
                table: "Consents");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Purposes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Purposes");

            migrationBuilder.DropColumn(
                name: "PurposeId",
                table: "Consents");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Purposes",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Consents",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Consents",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PurposeTitle",
                table: "Consents",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Purposes",
                table: "Purposes",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Consents_PurposeTitle",
                table: "Consents",
                column: "PurposeTitle");

            migrationBuilder.AddForeignKey(
                name: "FK_Consents_AspNetUsers_ApplicationUserId",
                table: "Consents",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Consents_Purposes_PurposeTitle",
                table: "Consents",
                column: "PurposeTitle",
                principalTable: "Purposes",
                principalColumn: "Title",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
