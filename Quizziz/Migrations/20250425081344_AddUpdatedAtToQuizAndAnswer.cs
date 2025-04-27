using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizziz.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdatedAtToQuizAndAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Quizzes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Quizzes",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Quizzes");
        }
    }
}
