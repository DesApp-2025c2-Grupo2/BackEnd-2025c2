using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefactorAndUpdateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HISTORIAL_TERAPEUTICO");

            migrationBuilder.DropColumn(
                name: "DURACIONCONSULTA",
                table: "AGENDAS");

            migrationBuilder.DropColumn(
                name: "ESPECIALIDADID",
                table: "AGENDAS");

            migrationBuilder.AddColumn<int>(
                name: "DURACIONCONSULTA",
                table: "HORARIOS_ATENCION",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ESPECIALIDADID",
                table: "HORARIOS_ATENCION",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HISTORIALESTERAPEUTICOS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PERSONAID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    SITUACIONTERAPEUTICAID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    FECHAINICIO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    FECHAFIN = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HISTORIALESTERAPEUTICOS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HISTORIALESTERAPEUTICOS_PERSONAS_PERSONAID",
                        column: x => x.PERSONAID,
                        principalTable: "PERSONAS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HISTORIALESTERAPEUTICOS_SITUACIONES_TERAPEUTICAS_SITUACIONTERAPEUTICAID",
                        column: x => x.SITUACIONTERAPEUTICAID,
                        principalTable: "SITUACIONES_TERAPEUTICAS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "REPORTES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODIGOIDENTIFICATORIO = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: false),
                    TIPO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    FECHADESDE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    FECHAHASTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    AFILIADOID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    FECHAGENERACION = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REPORTES", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HISTORIALESTERAPEUTICOS_PERSONAID",
                table: "HISTORIALESTERAPEUTICOS",
                column: "PERSONAID");

            migrationBuilder.CreateIndex(
                name: "IX_HISTORIALESTERAPEUTICOS_SITUACIONTERAPEUTICAID",
                table: "HISTORIALESTERAPEUTICOS",
                column: "SITUACIONTERAPEUTICAID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HISTORIALESTERAPEUTICOS");

            migrationBuilder.DropTable(
                name: "REPORTES");

            migrationBuilder.DropColumn(
                name: "DURACIONCONSULTA",
                table: "HORARIOS_ATENCION");

            migrationBuilder.DropColumn(
                name: "ESPECIALIDADID",
                table: "HORARIOS_ATENCION");

            migrationBuilder.AddColumn<int>(
                name: "DURACIONCONSULTA",
                table: "AGENDAS",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ESPECIALIDADID",
                table: "AGENDAS",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HISTORIAL_TERAPEUTICO",
                columns: table => new
                {
                    PERSONAID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    SITUACIONTERAPEUTICAID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HISTORIAL_TERAPEUTICO", x => new { x.PERSONAID, x.SITUACIONTERAPEUTICAID });
                    table.ForeignKey(
                        name: "FK_HISTORIAL_TERAPEUTICO_PERSONAS_PERSONAID",
                        column: x => x.PERSONAID,
                        principalTable: "PERSONAS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HISTORIAL_TERAPEUTICO_SITUACIONES_TERAPEUTICAS_SITUACIONTERAPEUTICAID",
                        column: x => x.SITUACIONTERAPEUTICAID,
                        principalTable: "SITUACIONES_TERAPEUTICAS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HISTORIAL_TERAPEUTICO_SITUACIONTERAPEUTICAID",
                table: "HISTORIAL_TERAPEUTICO",
                column: "SITUACIONTERAPEUTICAID");
        }
    }
}
