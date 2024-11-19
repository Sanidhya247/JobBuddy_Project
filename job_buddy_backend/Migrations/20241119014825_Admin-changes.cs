using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace job_buddy_backend.Migrations
{
    /// <inheritdoc />
    public partial class Adminchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppliedVia",
                table: "Applications");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "JobListings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Dob",
                table: "Applications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Linkedin",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Applications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "Dob",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Linkedin",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Applications");

            migrationBuilder.AddColumn<string>(
                name: "AppliedVia",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
