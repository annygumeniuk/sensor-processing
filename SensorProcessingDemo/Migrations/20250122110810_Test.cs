using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SensorProcessingDemo.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Value",
                table: "Sensors",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Sensors",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
