using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace job_buddy_backend.Migrations
{
    /// <inheritdoc />
    public partial class Addedjoblistingmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "JobListings",
                newName: "ZipCode");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "JobListings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PayRatePerHour",
                table: "JobListings",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PayRatePerYear",
                table: "JobListings",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "JobListings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShortJobDescription",
                table: "JobListings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkType",
                table: "JobListings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "PayRatePerHour",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "PayRatePerYear",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "ShortJobDescription",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "WorkType",
                table: "JobListings");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "JobListings",
                newName: "Location");
        }
    }
}
