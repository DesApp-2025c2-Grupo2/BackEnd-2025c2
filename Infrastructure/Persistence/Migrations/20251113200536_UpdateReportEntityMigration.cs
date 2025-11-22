using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReportEntityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_REPORTES",
                table: "REPORTES");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "REPORTES");

            migrationBuilder.AlterColumn<string>(
                name: "CODIGOIDENTIFICATORIO",
                table: "REPORTES",
                type: "NVARCHAR2(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(8)",
                oldMaxLength: 8);

            migrationBuilder.AddPrimaryKey(
                name: "PK_REPORTES",
                table: "REPORTES",
                column: "CODIGOIDENTIFICATORIO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_REPORTES",
                table: "REPORTES");

            migrationBuilder.AlterColumn<string>(
                name: "CODIGOIDENTIFICATORIO",
                table: "REPORTES",
                type: "NVARCHAR2(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(450)");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "REPORTES",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0)
                .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_REPORTES",
                table: "REPORTES",
                column: "ID");
        }
    }
}
