using Microsoft.EntityFrameworkCore.Migrations;

namespace AcademyManager.Migrations
{
    public partial class AddedTraineeIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_AspNetUsers_StudentId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_StudentId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "TraineeId",
                table: "Scores",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TraineeId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scores_TraineeId",
                table: "Scores",
                column: "TraineeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_AspNetUsers_TraineeId",
                table: "Scores",
                column: "TraineeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_AspNetUsers_TraineeId",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_TraineeId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "TraineeId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "TraineeId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "Scores",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scores_StudentId",
                table: "Scores",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_AspNetUsers_StudentId",
                table: "Scores",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
