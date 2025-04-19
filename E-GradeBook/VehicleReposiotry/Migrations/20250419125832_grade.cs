using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleReposiotry.Migrations
{
    /// <inheritdoc />
    public partial class grade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    firsSemester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstSemesterFinal = table.Column<int>(type: "int", nullable: true),
                    lastSemesterFinal = table.Column<int>(type: "int", nullable: true),
                    lastSemester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    finalGrade = table.Column<int>(type: "int", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ApplicationUserId",
                table: "Grades",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Grades");
        }
    }
}
