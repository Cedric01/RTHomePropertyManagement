using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RTHomePropertyManagement.Migrations.RealEstateDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "core");

            migrationBuilder.CreateTable(
                name: "agents",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    designation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    image_url = table.Column<string>(type: "text", nullable: true),
                    profile_link = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "listing_types",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listing_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    display_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "price_ranges",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    min_price = table.Column<decimal>(type: "numeric", nullable: false),
                    max_price = table.Column<decimal>(type: "numeric", nullable: false),
                    display_label = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_price_ranges", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "properties",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    location_id = table.Column<int>(type: "integer", nullable: true),
                    listing_type_id = table.Column<int>(type: "integer", nullable: true),
                    agent_id = table.Column<int>(type: "integer", nullable: true),
                    price_range_id = table.Column<int>(type: "integer", nullable: true),
                    is_for_rent = table.Column<bool>(type: "boolean", nullable: false),
                    price = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    price_period = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    square_feet = table.Column<int>(type: "integer", nullable: true),
                    bedrooms = table.Column<int>(type: "integer", nullable: true),
                    bathrooms = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_properties", x => x.id);
                    table.ForeignKey(
                        name: "FK_properties_agents_agent_id",
                        column: x => x.agent_id,
                        principalSchema: "core",
                        principalTable: "agents",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_properties_listing_types_listing_type_id",
                        column: x => x.listing_type_id,
                        principalSchema: "core",
                        principalTable: "listing_types",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_properties_locations_location_id",
                        column: x => x.location_id,
                        principalSchema: "core",
                        principalTable: "locations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_properties_price_ranges_price_range_id",
                        column: x => x.price_range_id,
                        principalSchema: "core",
                        principalTable: "price_ranges",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "property_images",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    property_id = table.Column<int>(type: "integer", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_property_images", x => x.id);
                    table.ForeignKey(
                        name: "FK_property_images_properties_property_id",
                        column: x => x.property_id,
                        principalSchema: "core",
                        principalTable: "properties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_properties_agent_id",
                schema: "core",
                table: "properties",
                column: "agent_id");

            migrationBuilder.CreateIndex(
                name: "IX_properties_listing_type_id",
                schema: "core",
                table: "properties",
                column: "listing_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_properties_location_id",
                schema: "core",
                table: "properties",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_properties_price_range_id",
                schema: "core",
                table: "properties",
                column: "price_range_id");

            migrationBuilder.CreateIndex(
                name: "IX_property_images_property_id",
                schema: "core",
                table: "property_images",
                column: "property_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "property_images",
                schema: "core");

            migrationBuilder.DropTable(
                name: "properties",
                schema: "core");

            migrationBuilder.DropTable(
                name: "agents",
                schema: "core");

            migrationBuilder.DropTable(
                name: "listing_types",
                schema: "core");

            migrationBuilder.DropTable(
                name: "locations",
                schema: "core");

            migrationBuilder.DropTable(
                name: "price_ranges",
                schema: "core");
        }
    }
}
