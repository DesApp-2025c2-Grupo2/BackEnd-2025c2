using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrestadoresSchemaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PRESTADORES_PRESTADORES_CENTROID",
                table: "PRESTADORES");

            migrationBuilder.DropColumn(
                name: "CENTROMEDICO",
                table: "PRESTADORES");

            migrationBuilder.DropColumn(
                name: "ROL",
                table: "PRESTADORES");

            migrationBuilder.DropColumn(
                name: "ALTA",
                table: "HORARIOS_ATENCION");

            migrationBuilder.DropColumn(
                name: "BAJA",
                table: "HORARIOS_ATENCION");

            migrationBuilder.DropColumn(
                name: "ALTA",
                table: "AGENDAS");

            migrationBuilder.DropColumn(
                name: "BAJA",
                table: "AGENDAS");

            migrationBuilder.DropColumn(
                name: "DIRECCION",
                table: "AGENDAS");

            migrationBuilder.DropColumn(
                name: "DURACIONCONSULTA",
                table: "AGENDAS");

            migrationBuilder.RenameColumn(
                name: "DURACIONCONSULTA",
                table: "HORARIOS_ATENCION",
                newName: "ORDEN");

            migrationBuilder.RenameColumn(
                name: "DIADEATENCION",
                table: "HORARIOS_ATENCION",
                newName: "DURACIONCONSULTAMINUTOS");

            migrationBuilder.RenameColumn(
                name: "ESPECIALIDADID",
                table: "AGENDAS",
                newName: "DIRECCIONID");

            migrationBuilder.AlterColumn<string>(
                name: "BAJA",
                table: "PRESTADORES",
                type: "NVARCHAR2(10)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ALTA",
                table: "PRESTADORES",
                type: "NVARCHAR2(10)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(7)");

            migrationBuilder.AddColumn<string>(
                name: "DISCRIMINATOR",
                table: "PRESTADORES",
                type: "NVARCHAR2(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MATRICULA",
                table: "PRESTADORES",
                type: "NVARCHAR2(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RAZONSOCIAL",
                table: "PRESTADORES",
                type: "NVARCHAR2(2000)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HORAINICIO",
                table: "HORARIOS_ATENCION",
                type: "NVARCHAR2(48)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(7)");

            migrationBuilder.AlterColumn<string>(
                name: "HORAFIN",
                table: "HORARIOS_ATENCION",
                type: "NVARCHAR2(48)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(7)");

            migrationBuilder.AddColumn<int>(
                name: "DIA",
                table: "HORARIOS_ATENCION",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PROFESIONALASIGNADOID",
                table: "HORARIOS_ATENCION",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PROFESIONALID",
                table: "AGENDAS",
                type: "NUMBER(10)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.AddColumn<int>(
                name: "CENTROMEDICOID",
                table: "AGENDAS",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DISCRIMINATOR",
                table: "AGENDAS",
                type: "NVARCHAR2(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_HORARIOS_ATENCION_ESPECIALIDADID",
                table: "HORARIOS_ATENCION",
                column: "ESPECIALIDADID");

            migrationBuilder.CreateIndex(
                name: "IX_HORARIOS_ATENCION_PROFESIONALASIGNADOID",
                table: "HORARIOS_ATENCION",
                column: "PROFESIONALASIGNADOID");

            migrationBuilder.CreateIndex(
                name: "IX_AGENDAS_CENTROMEDICOID",
                table: "AGENDAS",
                column: "CENTROMEDICOID");

            migrationBuilder.CreateIndex(
                name: "IX_AGENDAS_DIRECCIONID",
                table: "AGENDAS",
                column: "DIRECCIONID");

            migrationBuilder.AddForeignKey(
                name: "FK_AGENDAS_DIRECCIONES_DIRECCIONID",
                table: "AGENDAS",
                column: "DIRECCIONID",
                principalTable: "DIRECCIONES",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AGENDAS_PRESTADORES_CENTROMEDICOID",
                table: "AGENDAS",
                column: "CENTROMEDICOID",
                principalTable: "PRESTADORES",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HORARIOS_ATENCION_ESPECIALIDADES_ESPECIALIDADID",
                table: "HORARIOS_ATENCION",
                column: "ESPECIALIDADID",
                principalTable: "ESPECIALIDADES",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HORARIOS_ATENCION_PRESTADORES_PROFESIONALASIGNADOID",
                table: "HORARIOS_ATENCION",
                column: "PROFESIONALASIGNADOID",
                principalTable: "PRESTADORES",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PRESTADORES_PRESTADORES_CENTROID",
                table: "PRESTADORES",
                column: "CENTROID",
                principalTable: "PRESTADORES",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AGENDAS_DIRECCIONES_DIRECCIONID",
                table: "AGENDAS");

            migrationBuilder.DropForeignKey(
                name: "FK_AGENDAS_PRESTADORES_CENTROMEDICOID",
                table: "AGENDAS");

            migrationBuilder.DropForeignKey(
                name: "FK_HORARIOS_ATENCION_ESPECIALIDADES_ESPECIALIDADID",
                table: "HORARIOS_ATENCION");

            migrationBuilder.DropForeignKey(
                name: "FK_HORARIOS_ATENCION_PRESTADORES_PROFESIONALASIGNADOID",
                table: "HORARIOS_ATENCION");

            migrationBuilder.DropForeignKey(
                name: "FK_PRESTADORES_PRESTADORES_CENTROID",
                table: "PRESTADORES");

            migrationBuilder.DropIndex(
                name: "IX_HORARIOS_ATENCION_ESPECIALIDADID",
                table: "HORARIOS_ATENCION");

            migrationBuilder.DropIndex(
                name: "IX_HORARIOS_ATENCION_PROFESIONALASIGNADOID",
                table: "HORARIOS_ATENCION");

            migrationBuilder.DropIndex(
                name: "IX_AGENDAS_CENTROMEDICOID",
                table: "AGENDAS");

            migrationBuilder.DropIndex(
                name: "IX_AGENDAS_DIRECCIONID",
                table: "AGENDAS");

            migrationBuilder.DropColumn(
                name: "DISCRIMINATOR",
                table: "PRESTADORES");

            migrationBuilder.DropColumn(
                name: "MATRICULA",
                table: "PRESTADORES");

            migrationBuilder.DropColumn(
                name: "RAZONSOCIAL",
                table: "PRESTADORES");

            migrationBuilder.DropColumn(
                name: "DIA",
                table: "HORARIOS_ATENCION");

            migrationBuilder.DropColumn(
                name: "PROFESIONALASIGNADOID",
                table: "HORARIOS_ATENCION");

            migrationBuilder.DropColumn(
                name: "CENTROMEDICOID",
                table: "AGENDAS");

            migrationBuilder.DropColumn(
                name: "DISCRIMINATOR",
                table: "AGENDAS");

            migrationBuilder.RenameColumn(
                name: "ORDEN",
                table: "HORARIOS_ATENCION",
                newName: "DURACIONCONSULTA");

            migrationBuilder.RenameColumn(
                name: "DURACIONCONSULTAMINUTOS",
                table: "HORARIOS_ATENCION",
                newName: "DIADEATENCION");

            migrationBuilder.RenameColumn(
                name: "DIRECCIONID",
                table: "AGENDAS",
                newName: "ESPECIALIDADID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BAJA",
                table: "PRESTADORES",
                type: "TIMESTAMP(7)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ALTA",
                table: "PRESTADORES",
                type: "TIMESTAMP(7)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(10)");

            migrationBuilder.AddColumn<string>(
                name: "CENTROMEDICO",
                table: "PRESTADORES",
                type: "NVARCHAR2(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ROL",
                table: "PRESTADORES",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "HORAINICIO",
                table: "HORARIOS_ATENCION",
                type: "TIMESTAMP(7)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(48)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "HORAFIN",
                table: "HORARIOS_ATENCION",
                type: "TIMESTAMP(7)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(48)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ALTA",
                table: "HORARIOS_ATENCION",
                type: "TIMESTAMP(7)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "BAJA",
                table: "HORARIOS_ATENCION",
                type: "TIMESTAMP(7)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PROFESIONALID",
                table: "AGENDAS",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ALTA",
                table: "AGENDAS",
                type: "TIMESTAMP(7)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "BAJA",
                table: "AGENDAS",
                type: "TIMESTAMP(7)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DIRECCION",
                table: "AGENDAS",
                type: "NVARCHAR2(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DURACIONCONSULTA",
                table: "AGENDAS",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PRESTADORES_PRESTADORES_CENTROID",
                table: "PRESTADORES",
                column: "CENTROID",
                principalTable: "PRESTADORES",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
