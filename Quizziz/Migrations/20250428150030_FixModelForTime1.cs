using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizziz.Migrations
{
    /// <inheritdoc />
    public partial class FixModelForTime1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 28, 15, 0, 29, 503, DateTimeKind.Utc).AddTicks(9800));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 28, 15, 0, 29, 504, DateTimeKind.Utc).AddTicks(356));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 28, 15, 0, 29, 504, DateTimeKind.Utc).AddTicks(358));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 28, 14, 58, 53, 962, DateTimeKind.Utc).AddTicks(4864));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 28, 14, 58, 53, 962, DateTimeKind.Utc).AddTicks(5098));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 28, 14, 58, 53, 962, DateTimeKind.Utc).AddTicks(5099));
        }
    }
}
