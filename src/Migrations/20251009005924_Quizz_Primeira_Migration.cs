using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TigreDoMexico.Quizz.Api.Migrations
{
    /// <inheritdoc />
    public partial class Quizz_Primeira_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pergunta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Enunciado = table.Column<string>(type: "text", nullable: false),
                    Categoria = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pergunta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "resposta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Enunciado = table.Column<string>(type: "text", nullable: false),
                    Correta = table.Column<bool>(type: "boolean", nullable: false),
                    PerguntaId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resposta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_resposta_pergunta_PerguntaId",
                        column: x => x.PerguntaId,
                        principalTable: "pergunta",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_resposta_PerguntaId",
                table: "resposta",
                column: "PerguntaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "resposta");

            migrationBuilder.DropTable(
                name: "pergunta");
        }
    }
}
