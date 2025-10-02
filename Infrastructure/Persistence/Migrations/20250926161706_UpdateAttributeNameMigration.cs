using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAttributeNameMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HISTORIAL_TERAPEUTICO_SITUACIONES_TERAPEUTICAS_SITUCACIONTERAPEUTICAID",
                table: "HISTORIAL_TERAPEUTICO");

            migrationBuilder.DropTable(
                name: "PRESTADORES_ESPECIALIDADES");

            migrationBuilder.RenameColumn(
                name: "SITUCACIONTERAPEUTICAID",
                table: "HISTORIAL_TERAPEUTICO",
                newName: "SITUACIONTERAPEUTICAID");

            migrationBuilder.RenameIndex(
                name: "IX_HISTORIAL_TERAPEUTICO_SITUCACIONTERAPEUTICAID",
                table: "HISTORIAL_TERAPEUTICO",
                newName: "IX_HISTORIAL_TERAPEUTICO_SITUACIONTERAPEUTICAID");

            migrationBuilder.CreateTable(
                name: "ESPECIALIZACIONES",
                columns: table => new
                {
                    ESPECIALIDADID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PRESTADORID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESPECIALIZACIONES", x => new { x.ESPECIALIDADID, x.PRESTADORID });
                    table.ForeignKey(
                        name: "FK_ESPECIALIZACIONES_ESPECIALIDADES_ESPECIALIDADID",
                        column: x => x.ESPECIALIDADID,
                        principalTable: "ESPECIALIDADES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ESPECIALIZACIONES_PRESTADORES_PRESTADORID",
                        column: x => x.PRESTADORID,
                        principalTable: "PRESTADORES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ESPECIALIZACIONES_PRESTADORID",
                table: "ESPECIALIZACIONES",
                column: "PRESTADORID");

            migrationBuilder.AddForeignKey(
                name: "FK_HISTORIAL_TERAPEUTICO_SITUACIONES_TERAPEUTICAS_SITUACIONTERAPEUTICAID",
                table: "HISTORIAL_TERAPEUTICO",
                column: "SITUACIONTERAPEUTICAID",
                principalTable: "SITUACIONES_TERAPEUTICAS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HISTORIAL_TERAPEUTICO_SITUACIONES_TERAPEUTICAS_SITUACIONTERAPEUTICAID",
                table: "HISTORIAL_TERAPEUTICO");

            migrationBuilder.DropTable(
                name: "ESPECIALIZACIONES");

            migrationBuilder.RenameColumn(
                name: "SITUACIONTERAPEUTICAID",
                table: "HISTORIAL_TERAPEUTICO",
                newName: "SITUCACIONTERAPEUTICAID");

            migrationBuilder.RenameIndex(
                name: "IX_HISTORIAL_TERAPEUTICO_SITUACIONTERAPEUTICAID",
                table: "HISTORIAL_TERAPEUTICO",
                newName: "IX_HISTORIAL_TERAPEUTICO_SITUCACIONTERAPEUTICAID");

            migrationBuilder.CreateTable(
                name: "PRESTADORES_ESPECIALIDADES",
                columns: table => new
                {
                    ESPECIALIDADID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PRESTADORID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRESTADORES_ESPECIALIDADES", x => new { x.ESPECIALIDADID, x.PRESTADORID });
                    table.ForeignKey(
                        name: "FK_PRESTADORES_ESPECIALIDADES_ESPECIALIDADES_ESPECIALIDADID",
                        column: x => x.ESPECIALIDADID,
                        principalTable: "ESPECIALIDADES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PRESTADORES_ESPECIALIDADES_PRESTADORES_PRESTADORID",
                        column: x => x.PRESTADORID,
                        principalTable: "PRESTADORES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PRESTADORES_ESPECIALIDADES_PRESTADORID",
                table: "PRESTADORES_ESPECIALIDADES",
                column: "PRESTADORID");

            migrationBuilder.AddForeignKey(
                name: "FK_HISTORIAL_TERAPEUTICO_SITUACIONES_TERAPEUTICAS_SITUCACIONTERAPEUTICAID",
                table: "HISTORIAL_TERAPEUTICO",
                column: "SITUCACIONTERAPEUTICAID",
                principalTable: "SITUACIONES_TERAPEUTICAS",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
