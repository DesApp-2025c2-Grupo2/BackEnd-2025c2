using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatePrestadorEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                principalColumn: "ID");
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
        }
    }
}
