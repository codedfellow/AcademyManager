﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace AcademyManager.Migrations
{
    public partial class AddedResultReadyColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TraineeSerialStores");

            migrationBuilder.CreateTable(
                name: "AppStateStore",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Serial = table.Column<int>(nullable: false),
                    ResultReady = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppStateStore", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppStateStore");

            migrationBuilder.CreateTable(
                name: "TraineeSerialStores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Serial = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraineeSerialStores", x => x.Id);
                });
        }
    }
}
