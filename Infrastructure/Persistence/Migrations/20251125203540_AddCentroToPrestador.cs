using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCentroToPrestador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CODIGOIDENTIFICATORIO",
                table: "REPORTES",
                type: "CHAR(8)",
                fixedLength: true,
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(450)");

            migrationBuilder.AddColumn<int>(
                name: "CENTROID",
                table: "PRESTADORES",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PRESTADORES_CENTROID",
                table: "PRESTADORES",
                column: "CENTROID");

            migrationBuilder.AddForeignKey(
                name: "FK_PRESTADORES_PRESTADORES_CENTROID",
                table: "PRESTADORES",
                column: "CENTROID",
                principalTable: "PRESTADORES",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PRESTADORES_PRESTADORES_CENTROID",
                table: "PRESTADORES");

            migrationBuilder.DropIndex(
                name: "IX_PRESTADORES_CENTROID",
                table: "PRESTADORES");

            migrationBuilder.DropColumn(
                name: "CENTROID",
                table: "PRESTADORES");

            migrationBuilder.AlterColumn<string>(
                name: "CODIGOIDENTIFICATORIO",
                table: "REPORTES",
                type: "NVARCHAR2(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "CHAR(8)",
                oldFixedLength: true,
                oldMaxLength: 8);
        }
    }
}
