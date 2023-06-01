using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MegaDeskRazor.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Desk",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DeskWidth = table.Column<int>(type: "int", nullable: false),
                    DeskDepth = table.Column<int>(type: "int", nullable: false),
                    DeskNumDrawers = table.Column<int>(type: "int", nullable: false),
                    SurfaceMaterialIndex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingTypeIndex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalArea = table.Column<int>(type: "int", nullable: false),
                    TotalSurfaceCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDrawerCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SurfaceMaterialType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalSurfaceMaterialCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Desk", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Desk");
        }
    }
}
