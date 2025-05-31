using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Voluta.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaInsertDeUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "AreasInteresseJson", "DataCadastro", "Disponivel", "Email", "Nome", "Role", "SenhaHash", "Telefone" },
                values: new object[,]
                {
                    { 1, "[0]", new DateTime(2025, 5, 31, 13, 24, 46, 244, DateTimeKind.Local).AddTicks(6559), false, "admin@voluta.com", "Admin", 0, "$2a$11$jaVwxXX/oNn2BmFdmbnSOeGUknB3rlESw/Mfo0m/0KWJ8dx0tzbnW", "(11) 99999-9999" },
                    { 2, "[0]", new DateTime(2025, 5, 31, 13, 24, 46, 346, DateTimeKind.Local).AddTicks(2167), false, "representante@ong.com", "Representante ONG", 2, "$2a$11$QgQ1Q9vlakxFwnn.N2h6LeedwQJPbN0/N72nvUV6oUpV/86ZAqQIu", "(11) 88888-8888" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
