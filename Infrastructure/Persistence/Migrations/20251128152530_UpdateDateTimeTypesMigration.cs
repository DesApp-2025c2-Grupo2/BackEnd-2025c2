using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDateTimeTypesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
