using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleReposiotry.Migrations
{
    /// <inheritdoc />
    public partial class initnewbase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubjectProfessors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectProfessors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectProfessors_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectProfessors_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubjectStudents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubjectProfessorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectStudents_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubjectStudents_SubjectProfessors_SubjectProfessorId",
                        column: x => x.SubjectProfessorId,
                        principalTable: "SubjectProfessors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectProfessors_ApplicationUserId",
                table: "SubjectProfessors",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectProfessors_SubjectId",
                table: "SubjectProfessors",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectStudents_ApplicationUserId",
                table: "SubjectStudents",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectStudents_SubjectProfessorId",
                table: "SubjectStudents",
                column: "SubjectProfessorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectStudents");

            migrationBuilder.DropTable(
                name: "SubjectProfessors");
        }
    }
}
