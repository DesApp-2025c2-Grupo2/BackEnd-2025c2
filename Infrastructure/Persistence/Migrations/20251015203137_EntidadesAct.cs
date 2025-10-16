using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EntidadesAct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DOCUMENTACIONES_PERSONAID",
                table: "DOCUMENTACIONES");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENTACIONES_PERSONAID",
                table: "DOCUMENTACIONES",
                column: "PERSONAID",
                unique: true,
                filter: "\"PERSONAID\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DOCUMENTACIONES_PERSONAID",
                table: "DOCUMENTACIONES");

            migrationBuilder.CreateIndex(
                name: "IX_DOCUMENTACIONES_PERSONAID",
                table: "DOCUMENTACIONES",
                column: "PERSONAID");
        }
    }
}
