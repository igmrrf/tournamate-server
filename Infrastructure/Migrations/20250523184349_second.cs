using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Fouls_MatchFoulsId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Performance_TeamPerformanceId",
                table: "Match");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeamPerformanceId",
                table: "Match",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AlterColumn<Guid>(
                name: "MatchFoulsId",
                table: "Match",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Fouls_MatchFoulsId",
                table: "Match",
                column: "MatchFoulsId",
                principalTable: "Fouls",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Performance_TeamPerformanceId",
                table: "Match",
                column: "TeamPerformanceId",
                principalTable: "Performance",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Fouls_MatchFoulsId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Performance_TeamPerformanceId",
                table: "Match");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TeamPerformanceId",
                table: "Match",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MatchFoulsId",
                table: "Match",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Fouls_MatchFoulsId",
                table: "Match",
                column: "MatchFoulsId",
                principalTable: "Fouls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Performance_TeamPerformanceId",
                table: "Match",
                column: "TeamPerformanceId",
                principalTable: "Performance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
