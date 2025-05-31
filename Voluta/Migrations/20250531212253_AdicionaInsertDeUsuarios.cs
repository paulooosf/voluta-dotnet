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
                    { 1, "[0]", new DateTime(2025, 5, 31, 18, 22, 52, 587, DateTimeKind.Local).AddTicks(1451), false, "usuario@voluta.com", "Usuario", 1, "$2a$11$sM2r6Ennv1KTPT5sfgD5a.cB0H3iZqLA502MaTs02jZYfxhwy7ZaW", "(11) 99999-9999" },
                    { 2, "[0]", new DateTime(2025, 5, 31, 18, 22, 52, 685, DateTimeKind.Local).AddTicks(2324), false, "admin@voluta.com", "Admin", 0, "$2a$11$4tkN3MP8BY1da1E5rW3HNeBVLe1kg65X9.usgpVVw0VCEGRktxgym", "(11) 99999-9999" },
                    { 3, "[0]", new DateTime(2025, 5, 31, 18, 22, 52, 783, DateTimeKind.Local).AddTicks(7845), false, "representante@ong.com", "Representante ONG", 2, "$2a$11$bwCCxOrJPRuHzamsAGhAOuZGt8pAHP.zFeaJz2ORTEEGOw3gtfb6S", "(11) 88888-8888" }
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

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
