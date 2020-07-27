using Microsoft.EntityFrameworkCore.Migrations;

namespace JWTPolicyBasedAuthorization.Migrations
{
    public partial class NewTablesConfigKeyValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigKey",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(maxLength: 20, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigKey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigValue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KeyId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(maxLength: 120, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Rank = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfigValue_ConfigKey_KeyId",
                        column: x => x.KeyId,
                        principalTable: "ConfigKey",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigValue_KeyId",
                table: "ConfigValue",
                column: "KeyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigValue");

            migrationBuilder.DropTable(
                name: "ConfigKey");
        }
    }
}
