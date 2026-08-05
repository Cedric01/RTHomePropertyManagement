using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RTHomePropertyManagement.Migrations.RealEstateDb
{
    /// <inheritdoc />
    public partial class AddPropertyFeatureFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ceiling_height",
                schema: "core",
                table: "properties",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "disabled_access",
                schema: "core",
                table: "properties",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "floor",
                schema: "core",
                table: "properties",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "furnishing",
                schema: "core",
                table: "properties",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "garage",
                schema: "core",
                table: "properties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "garden_size",
                schema: "core",
                table: "properties",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "has_air_condition",
                schema: "core",
                table: "properties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "has_cable_tv",
                schema: "core",
                table: "properties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "has_elevator",
                schema: "core",
                table: "properties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "has_fence",
                schema: "core",
                table: "properties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "has_fireplace",
                schema: "core",
                table: "properties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "has_intercom",
                schema: "core",
                table: "properties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "has_swimming_pool",
                schema: "core",
                table: "properties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "has_ventilation",
                schema: "core",
                table: "properties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "has_wifi",
                schema: "core",
                table: "properties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "heating",
                schema: "core",
                table: "properties",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_pet_friendly",
                schema: "core",
                table: "properties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "parking_spots",
                schema: "core",
                table: "properties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "renovation",
                schema: "core",
                table: "properties",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "security",
                schema: "core",
                table: "properties",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "window_type",
                schema: "core",
                table: "properties",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "year_built",
                schema: "core",
                table: "properties",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ceiling_height",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "disabled_access",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "floor",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "furnishing",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "garage",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "garden_size",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "has_air_condition",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "has_cable_tv",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "has_elevator",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "has_fence",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "has_fireplace",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "has_intercom",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "has_swimming_pool",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "has_ventilation",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "has_wifi",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "heating",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "is_pet_friendly",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "parking_spots",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "renovation",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "security",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "window_type",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "year_built",
                schema: "core",
                table: "properties");
        }
    }
}
