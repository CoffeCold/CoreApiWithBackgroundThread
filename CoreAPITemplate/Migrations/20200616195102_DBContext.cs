using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreAPI.Migrations
{
    public partial class DBContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    JobId = table.Column<Guid>(nullable: false),
                    ExecutionDomain = table.Column<int>(nullable: false),
                    ScheduleDate = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    StopDate = table.Column<DateTime>(nullable: false),
                    JobProperty1 = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    JobState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.JobId);
                });

            migrationBuilder.CreateTable(
                name: "JobLogs",
                columns: table => new
                {
                    LogId = table.Column<Guid>(nullable: false),
                    Logcomment = table.Column<string>(nullable: true),
                    Logdate = table.Column<DateTime>(nullable: false),
                    JobId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLogs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_JobLogs_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobLogs_JobId",
                table: "JobLogs",
                column: "JobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobLogs");

            migrationBuilder.DropTable(
                name: "Jobs");
        }
    }
}
