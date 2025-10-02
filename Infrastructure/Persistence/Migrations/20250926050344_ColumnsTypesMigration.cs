using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ColumnsTypesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ROL",
                table: "PRESTADORES",
                type: "NUMBER(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<int>(
                name: "PARENTESCO",
                table: "PERSONAS",
                type: "NUMBER(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<int>(
                name: "NUMEROINTEGRANTE",
                table: "PERSONAS",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DIAATENCION",
                table: "HORARIOS_ATENCION",
                type: "NUMBER(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<int>(
                name: "TIPODOCUMENTO",
                table: "DOCUMENTACIONES",
                type: "NUMBER(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<int>(
                name: "NUMEROAFILIADO",
                table: "AFILIADOS",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NUMEROINTEGRANTE",
                table: "PERSONAS");

            migrationBuilder.DropColumn(
                name: "NUMEROAFILIADO",
                table: "AFILIADOS");

            migrationBuilder.AlterColumn<string>(
                name: "ROL",
                table: "PRESTADORES",
                type: "NVARCHAR2(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.AlterColumn<string>(
                name: "PARENTESCO",
                table: "PERSONAS",
                type: "NVARCHAR2(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.AlterColumn<string>(
                name: "DIAATENCION",
                table: "HORARIOS_ATENCION",
                type: "NVARCHAR2(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.AlterColumn<string>(
                name: "TIPODOCUMENTO",
                table: "DOCUMENTACIONES",
                type: "NVARCHAR2(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");
        }
    }
}
