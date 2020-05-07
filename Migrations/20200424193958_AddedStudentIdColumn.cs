using Microsoft.EntityFrameworkCore.Migrations;

namespace AcademyManager.Migrations
{
    public partial class AddedStudentIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChangePasswordVM",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CurrentPassword = table.Column<string>(nullable: false),
                    NewPassword = table.Column<string>(nullable: false),
                    ConfiremNewPassword = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangePasswordVM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EditProfileVM",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditProfileVM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfileVM",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileVM", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangePasswordVM");

            migrationBuilder.DropTable(
                name: "EditProfileVM");

            migrationBuilder.DropTable(
                name: "UserProfileVM");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "AspNetUsers");
        }
    }
}
