using Microsoft.EntityFrameworkCore.Migrations;

namespace CallingBotSample.Migrations
{
    public partial class InitialSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CallDetails",
                columns: table => new
                {
                    CallId = table.Column<string>(nullable: true),
                    ParticipantId = table.Column<string>(nullable: true),
                    ParticipantName = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallDetails");
        }
    }
}
