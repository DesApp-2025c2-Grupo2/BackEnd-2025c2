using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ESPECIALIDADES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOMBRE = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: false),
                    DESCRIPCION = table.Column<string>(type: "NVARCHAR2(512)", maxLength: 512, nullable: false),
                    ALTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    BAJA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESPECIALIDADES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PLANES_MEDICOS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOMBRE = table.Column<string>(type: "NVARCHAR2(32)", maxLength: 32, nullable: false),
                    DESCRIPCION = table.Column<string>(type: "NVARCHAR2(512)", maxLength: 512, nullable: false),
                    COSTOMENSUAL = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MONEDA = table.Column<string>(type: "NVARCHAR2(3)", maxLength: 3, nullable: false),
                    ALTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    BAJA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PLANES_MEDICOS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PRESTADORES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ROL = table.Column<string>(type: "NVARCHAR2(16)", maxLength: 16, nullable: false),
                    NOMBRECOMPLETO = table.Column<string>(type: "NVARCHAR2(128)", maxLength: 128, nullable: false),
                    CENTROMEDICO = table.Column<string>(type: "NVARCHAR2(128)", maxLength: 128, nullable: true),
                    ALTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    BAJA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRESTADORES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SITUACIONES_TERAPEUTICAS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOMBRE = table.Column<string>(type: "NVARCHAR2(128)", maxLength: 128, nullable: false),
                    DESCRIPCION = table.Column<string>(type: "NVARCHAR2(512)", maxLength: 512, nullable: false),
                    ALTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    BAJA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SITUACIONES_TERAPEUTICAS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AFILIADOS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TITULARID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PLANMEDICOID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ALTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    BAJA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AFILIADOS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AFILIADOS_PLANES_MEDICOS_PLANMEDICOID",
                        column: x => x.PLANMEDICOID,
                        principalTable: "PLANES_MEDICOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AGENDAS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PROFESIONALID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ESPECIALIDADID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DIRECCION = table.Column<string>(type: "NVARCHAR2(128)", maxLength: 128, nullable: false),
                    DURACIONCONSULTA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ALTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    BAJA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AGENDAS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AGENDAS_PRESTADORES_PROFESIONALID",
                        column: x => x.PROFESIONALID,
                        principalTable: "PRESTADORES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "PERSONAS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOMBRE = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: false),
                    APELLIDO = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: false),
                    FECHANACIMIENTO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    PARENTESCO = table.Column<string>(type: "NVARCHAR2(32)", maxLength: 32, nullable: false),
                    AFILIADOID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ALTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    BAJA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERSONAS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PERSONAS_AFILIADOS_AFILIADOID",
                        column: x => x.AFILIADOID,
                        principalTable: "AFILIADOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HORARIOS_ATENCION",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    AGENDAID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DIAATENCION = table.Column<string>(type: "NVARCHAR2(16)", maxLength: 16, nullable: false),
                    HORAINICIO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    HORAFIN = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ALTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    BAJA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HORARIOS_ATENCION", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HORARIOS_ATENCION_AGENDAS_AGENDAID",
                        column: x => x.AGENDAID,
                        principalTable: "AGENDAS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DIRECCIONES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CALLE = table.Column<string>(type: "NVARCHAR2(32)", maxLength: 32, nullable: false),
                    ALTURA = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: false),
                    PISO = table.Column<string>(type: "NVARCHAR2(16)", maxLength: 16, nullable: true),
                    DEPARTAMENTO = table.Column<string>(type: "NVARCHAR2(16)", maxLength: 16, nullable: true),
                    PROVINCIACIUDAD = table.Column<string>(type: "NVARCHAR2(128)", maxLength: 128, nullable: false),
                    PERSONAID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PRESTADORID = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DIRECCIONES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DIRECCIONES_PERSONAS_PERSONAID",
                        column: x => x.PERSONAID,
                        principalTable: "PERSONAS",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_DIRECCIONES_PRESTADORES_PRESTADORID",
                        column: x => x.PRESTADORID,
                        principalTable: "PRESTADORES",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "DOCUMENTACIONES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TIPODOCUMENTO = table.Column<string>(type: "NVARCHAR2(32)", maxLength: 32, nullable: false),
                    NUMERO = table.Column<string>(type: "NVARCHAR2(16)", maxLength: 16, nullable: false),
                    PERSONAID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PRESTADORID = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCUMENTACIONES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DOCUMENTACIONES_PERSONAS_PERSONAID",
                        column: x => x.PERSONAID,
                        principalTable: "PERSONAS",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_DOCUMENTACIONES_PRESTADORES_PRESTADORID",
                        column: x => x.PRESTADORID,
                        principalTable: "PRESTADORES",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "EMAILS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CORREO = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: false),
                    PERSONAID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PRESTADORID = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMAILS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EMAILS_PERSONAS_PERSONAID",
                        column: x => x.PERSONAID,
                        principalTable: "PERSONAS",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_EMAILS_PRESTADORES_PRESTADORID",
                        column: x => x.PRESTADORID,
                        principalTable: "PRESTADORES",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "HISTORIAL_TERAPEUTICO",
                columns: table => new
                {
                    PERSONAID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    SITUCACIONTERAPEUTICAID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HISTORIAL_TERAPEUTICO", x => new { x.PERSONAID, x.SITUCACIONTERAPEUTICAID });
                    table.ForeignKey(
                        name: "FK_HISTORIAL_TERAPEUTICO_PERSONAS_PERSONAID",
                        column: x => x.PERSONAID,
                        principalTable: "PERSONAS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HISTORIAL_TERAPEUTICO_SITUACIONES_TERAPEUTICAS_SITUCACIONTERAPEUTICAID",
                        column: x => x.SITUCACIONTERAPEUTICAID,
                        principalTable: "SITUACIONES_TERAPEUTICAS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TELEFONOS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NUMERO = table.Column<string>(type: "NVARCHAR2(16)", maxLength: 16, nullable: false),
                    PERSONAID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PRESTADORID = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TELEFONOS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TELEFONOS_PERSONAS_PERSONAID",
                        column: x => x.PERSONAID,
                        principalTable: "PERSONAS",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TELEFONOS_PRESTADORES_PRESTADORID",
                        column: x => x.PRESTADORID,
                        principalTable: "PRESTADORES",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AFILIADOS_PLANMEDICOID",
                table: "AFILIADOS",
                column: "PLANMEDICOID");

            migrationBuilder.CreateIndex(
                name: "IX_AGENDAS_PROFESIONALID",
                table: "AGENDAS",
                column: "PROFESIONALID");

            migrationBuilder.CreateIndex(
                name: "IX_DIRECCIONES_PERSONAID",
                table: "DIRECCIONES",
                column: "PERSONAID");

            migrationBuilder.CreateIndex(
                name: "IX_DIRECCIONES_PRESTADORID",
                table: "DIRECCIONES",
                column: "PRESTADORID");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENTACIONES_PERSONAID",
                table: "DOCUMENTACIONES",
                column: "PERSONAID");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENTACIONES_PRESTADORID",
                table: "DOCUMENTACIONES",
                column: "PRESTADORID");

            migrationBuilder.CreateIndex(
                name: "IX_EMAILS_PERSONAID",
                table: "EMAILS",
                column: "PERSONAID");

            migrationBuilder.CreateIndex(
                name: "IX_EMAILS_PRESTADORID",
                table: "EMAILS",
                column: "PRESTADORID");

            migrationBuilder.CreateIndex(
                name: "IX_HISTORIAL_TERAPEUTICO_SITUCACIONTERAPEUTICAID",
                table: "HISTORIAL_TERAPEUTICO",
                column: "SITUCACIONTERAPEUTICAID");

            migrationBuilder.CreateIndex(
                name: "IX_HORARIOS_ATENCION_AGENDAID",
                table: "HORARIOS_ATENCION",
                column: "AGENDAID");

            migrationBuilder.CreateIndex(
                name: "IX_PERSONAS_AFILIADOID",
                table: "PERSONAS",
                column: "AFILIADOID");

            migrationBuilder.CreateIndex(
                name: "IX_PRESTADORES_ESPECIALIDADES_PRESTADORID",
                table: "PRESTADORES_ESPECIALIDADES",
                column: "PRESTADORID");

            migrationBuilder.CreateIndex(
                name: "IX_TELEFONOS_PERSONAID",
                table: "TELEFONOS",
                column: "PERSONAID");

            migrationBuilder.CreateIndex(
                name: "IX_TELEFONOS_PRESTADORID",
                table: "TELEFONOS",
                column: "PRESTADORID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DIRECCIONES");

            migrationBuilder.DropTable(
                name: "DOCUMENTACIONES");

            migrationBuilder.DropTable(
                name: "EMAILS");

            migrationBuilder.DropTable(
                name: "HISTORIAL_TERAPEUTICO");

            migrationBuilder.DropTable(
                name: "HORARIOS_ATENCION");

            migrationBuilder.DropTable(
                name: "PRESTADORES_ESPECIALIDADES");

            migrationBuilder.DropTable(
                name: "TELEFONOS");

            migrationBuilder.DropTable(
                name: "SITUACIONES_TERAPEUTICAS");

            migrationBuilder.DropTable(
                name: "AGENDAS");

            migrationBuilder.DropTable(
                name: "ESPECIALIDADES");

            migrationBuilder.DropTable(
                name: "PERSONAS");

            migrationBuilder.DropTable(
                name: "PRESTADORES");

            migrationBuilder.DropTable(
                name: "AFILIADOS");

            migrationBuilder.DropTable(
                name: "PLANES_MEDICOS");
        }
    }
}
