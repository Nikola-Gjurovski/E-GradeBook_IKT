using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleReposiotry.Migrations
{
    /// <inheritdoc />
    public partial class vehicle_formula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehicleFormulas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    engines = table.Column<int>(type: "int", nullable: false),
                    chassis = table.Column<int>(type: "int", nullable: false),
                    doors = table.Column<int>(type: "int", nullable: false),
                    wheels = table.Column<int>(type: "int", nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleFormulas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleFormulas");
        }
    }
}
