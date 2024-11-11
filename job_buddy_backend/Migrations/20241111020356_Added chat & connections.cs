using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace job_buddy_backend.Migrations
{
    /// <inheritdoc />
    public partial class Addedchatconnections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppliedVia",
                table: "Applications");

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

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    ChatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobID = table.Column<int>(type: "int", nullable: false),
                    JobSeekerID = table.Column<int>(type: "int", nullable: false),
                    EmployerID = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.ChatID);
                    table.ForeignKey(
                        name: "FK_Chats_JobListings_JobID",
                        column: x => x.JobID,
                        principalTable: "JobListings",
                        principalColumn: "JobID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chats_Users_EmployerID",
                        column: x => x.EmployerID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chats_Users_JobSeekerID",
                        column: x => x.JobSeekerID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    ConnectionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestorID = table.Column<int>(type: "int", nullable: false),
                    RequesteeID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.ConnectionID);
                    table.ForeignKey(
                        name: "FK_Connections_Users_RequesteeID",
                        column: x => x.RequesteeID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Connections_Users_RequestorID",
                        column: x => x.RequestorID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatID = table.Column<int>(type: "int", nullable: false),
                    SenderID = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageID);
                    table.ForeignKey(
                        name: "FK_Messages_Chats_ChatID",
                        column: x => x.ChatID,
                        principalTable: "Chats",
                        principalColumn: "ChatID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderID",
                        column: x => x.SenderID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_EmployerID",
                table: "Chats",
                column: "EmployerID");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_JobID",
                table: "Chats",
                column: "JobID");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_JobSeekerID",
                table: "Chats",
                column: "JobSeekerID");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_RequesteeID",
                table: "Connections",
                column: "RequesteeID");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_RequestorID",
                table: "Connections",
                column: "RequestorID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatID",
                table: "Messages",
                column: "ChatID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderID",
                table: "Messages",
                column: "SenderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Chats");

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
