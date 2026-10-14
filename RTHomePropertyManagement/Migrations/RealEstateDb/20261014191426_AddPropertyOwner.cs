using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RTHomePropertyManagement.Migrations.RealEstateDb
{
    /// <inheritdoc />
    public partial class AddPropertyOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "owner_id",
                schema: "core",
                table: "properties",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                schema: "core",
                table: "properties",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.CreateIndex(
                name: "IX_properties_owner_id",
                schema: "core",
                table: "properties",
                column: "owner_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_properties_owner_id",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "owner_id",
                schema: "core",
                table: "properties");

            migrationBuilder.DropColumn(
                name: "xmin",
                schema: "core",
                table: "properties");
        }
    }
}
