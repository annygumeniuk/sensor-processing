using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SensorProcessingDemo.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertsCollector_Sensors_SensorId",
                table: "AlertsCollector");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sensors",
                table: "Sensors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Monitorings",
                table: "Monitorings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlertsCollector",
                table: "AlertsCollector");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Sensors",
                newName: "Sensor");

            migrationBuilder.RenameTable(
                name: "Monitorings",
                newName: "Monitoring");

            migrationBuilder.RenameTable(
                name: "AlertsCollector",
                newName: "AlertCollector");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Sensor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sensor",
                table: "Sensor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Monitoring",
                table: "Monitoring",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlertCollector",
                table: "AlertCollector",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensor_UserId",
                table: "Sensor",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertCollector_Sensor_SensorId",
                table: "AlertCollector",
                column: "SensorId",
                principalTable: "Sensor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensor_User_UserId",
                table: "Sensor",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertCollector_Sensor_SensorId",
                table: "AlertCollector");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensor_User_UserId",
                table: "Sensor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sensor",
                table: "Sensor");

            migrationBuilder.DropIndex(
                name: "IX_Sensor_UserId",
                table: "Sensor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Monitoring",
                table: "Monitoring");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlertCollector",
                table: "AlertCollector");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Sensor");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Sensor",
                newName: "Sensors");

            migrationBuilder.RenameTable(
                name: "Monitoring",
                newName: "Monitorings");

            migrationBuilder.RenameTable(
                name: "AlertCollector",
                newName: "AlertsCollector");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sensors",
                table: "Sensors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Monitorings",
                table: "Monitorings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlertsCollector",
                table: "AlertsCollector",
                column: "SensorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertsCollector_Sensors_SensorId",
                table: "AlertsCollector",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
