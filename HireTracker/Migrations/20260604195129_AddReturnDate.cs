using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddReturnDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerhead",
                table: "Bookings");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ReturnDate",
                table: "Bookings",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "Bookings");

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerhead",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
