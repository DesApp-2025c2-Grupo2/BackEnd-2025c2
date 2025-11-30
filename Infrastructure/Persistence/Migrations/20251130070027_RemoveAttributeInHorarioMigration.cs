using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAttributeInHorarioMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ORDEN",
                table: "HORARIOS_ATENCION");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ORDEN",
                table: "HORARIOS_ATENCION",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);
        }
    }
}
