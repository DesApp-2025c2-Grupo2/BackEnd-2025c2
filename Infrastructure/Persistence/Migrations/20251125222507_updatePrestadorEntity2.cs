using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatePrestadorEntity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PRESTADORES_PRESTADORES_CENTROID",
                table: "PRESTADORES");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PRESTADORES_PRESTADORES_CENTROID",
                table: "PRESTADORES",
                column: "CENTROID",
                principalTable: "PRESTADORES",
                principalColumn: "ID");
        }
    }
}
