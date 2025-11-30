using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RefactorAtributesTypesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DIA",
                table: "HORARIOS_ATENCION");

            migrationBuilder.CreateTable(
                name: "HORARIODIA",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DIA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    HORARIOID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HORARIODIA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HORARIODIA_HORARIOS_ATENCION_HORARIOID",
                        column: x => x.HORARIOID,
                        principalTable: "HORARIOS_ATENCION",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HORARIODIA_HORARIOID",
                table: "HORARIODIA",
                column: "HORARIOID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HORARIODIA");

            migrationBuilder.AddColumn<int>(
                name: "DIA",
                table: "HORARIOS_ATENCION",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);
        }
    }
}
