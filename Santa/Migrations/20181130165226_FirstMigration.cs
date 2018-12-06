using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Santa.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Pass = table.Column<string>(nullable: true),
                    Joinable = table.Column<bool>(nullable: false),
                    OwnerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Persons_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mappings",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false),
                    PersonId = table.Column<long>(nullable: false),
                    GifteeId = table.Column<long>(nullable: true),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mappings", x => new { x.GroupId, x.PersonId });
                    table.ForeignKey(
                        name: "FK_Mappings_Persons_GifteeId",
                        column: x => x.GifteeId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mappings_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mappings_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_OwnerId",
                table: "Groups",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Mappings_GifteeId",
                table: "Mappings",
                column: "GifteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Mappings_PersonId",
                table: "Mappings",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mappings");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
