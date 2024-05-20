using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CRUD_Persons.Migrations
{
    /// <inheritdoc />
    public partial class feedDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "ID", "Age", "Birthday", "LastName", "Name", "Occupation" },
                values: new object[,]
                {
                    { 135465, 20, new DateTime(2024, 5, 20, 14, 9, 58, 51, DateTimeKind.Local).AddTicks(7027), "Pato", "Elsa", "Tanke de BV" },
                    { 465465, 25, new DateTime(2024, 5, 20, 14, 9, 58, 51, DateTimeKind.Local).AddTicks(7151), "Galarga", "Elver", "Tanke de BV" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "ID",
                keyValue: 135465);

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "ID",
                keyValue: 465465);
        }
    }
}
