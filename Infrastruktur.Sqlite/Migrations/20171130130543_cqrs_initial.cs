using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Billmorro.Tests.Migrations
{
    public partial class cqrs_initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CQRS_Events",
                columns: table => new
                {
                    EventID = table.Column<Guid>(nullable: false),
                    CommandId = table.Column<Guid>(nullable: false),
                    CorrelationID = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    DeviceID = table.Column<Guid>(nullable: false),
                    EventSet = table.Column<int>(nullable: false),
                    EventType = table.Column<Guid>(nullable: false),
                    Modul = table.Column<string>(nullable: true),
                    ModulInstanceID = table.Column<Guid>(nullable: false),
                    Payload = table.Column<string>(nullable: true),
                    SessionID = table.Column<Guid>(nullable: false),
                    StreamID = table.Column<Guid>(nullable: false),
                    UserID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CQRS_Events", x => x.EventID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CQRS_Events");
        }
    }
}
