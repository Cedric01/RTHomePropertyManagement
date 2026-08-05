using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RTHomePropertyManagement.Migrations.RealEstateDb
{
    /// <inheritdoc />
    public partial class AddDescriptionAndAgentContactFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tables already exist — only adding the new columns
            migrationBuilder.AddColumn<string>(
                name: "description",
                schema: "core",
                table: "properties",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                schema: "core",
                table: "agents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone",
                schema: "core",
                table: "agents",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "location",
                schema: "core",
                table: "agents",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "description", schema: "core", table: "properties");
            migrationBuilder.DropColumn(name: "email",       schema: "core", table: "agents");
            migrationBuilder.DropColumn(name: "phone",       schema: "core", table: "agents");
            migrationBuilder.DropColumn(name: "location",    schema: "core", table: "agents");
        }
    }
}
