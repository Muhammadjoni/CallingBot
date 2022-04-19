using Microsoft.EntityFrameworkCore.Migrations;

namespace CallingBotSample.Migrations
{
    public partial class ParticipantsSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParticipantDetails",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    EmailId = table.Column<string>(nullable: true),
                    TenantId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParticipantDetails");
        }
    }
}
