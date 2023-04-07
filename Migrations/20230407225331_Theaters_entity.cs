using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesAPI.Migrations
{
    /// <inheritdoc />
    public partial class Theaters_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Theaters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theaters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoviesTheaters",
                columns: table => new
                {
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    TheatherId = table.Column<int>(type: "int", nullable: false),
                    TheaterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesTheaters", x => new { x.TheatherId, x.MovieId });
                    table.ForeignKey(
                        name: "FK_MoviesTheaters_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoviesTheaters_Theaters_TheaterId",
                        column: x => x.TheaterId,
                        principalTable: "Theaters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoviesTheaters_MovieId",
                table: "MoviesTheaters",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesTheaters_TheaterId",
                table: "MoviesTheaters",
                column: "TheaterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoviesTheaters");

            migrationBuilder.DropTable(
                name: "Theaters");
        }
    }
}
