using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RTHomePropertyManagement.Migrations
{
    /// <inheritdoc />
    public partial class newTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bathrooms",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Properties",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Properties",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Bedrooms",
                table: "Properties",
                newName: "AvailableUnits");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Laundry",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Wifi",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Laundry",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Wifi",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Properties",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Properties",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "AvailableUnits",
                table: "Properties",
                newName: "Bedrooms");

            migrationBuilder.AddColumn<int>(
                name: "Bathrooms",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Properties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(150)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
