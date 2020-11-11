using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auriculoterapia.Api.Migrations
{
    public partial class ValidacionCorreoExiste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Usuarios",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailExist",
                table: "Usuarios",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaConfirmacionCodigo",
                table: "Usuarios",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEnvioCorreoConfirmacion",
                table: "Usuarios",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "EmailExist",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaConfirmacionCodigo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaEnvioCorreoConfirmacion",
                table: "Usuarios");
        }
    }
}
