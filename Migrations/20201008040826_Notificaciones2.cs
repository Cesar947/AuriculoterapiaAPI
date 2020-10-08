using Microsoft.EntityFrameworkCore.Migrations;

namespace Auriculoterapia.Api.Migrations
{
    public partial class Notificaciones2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_Usuarios_UsuarioEmisorId",
                table: "Notificaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_Usuarios_UsuarioReceptorId",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_UsuarioEmisorId",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_UsuarioReceptorId",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "UsuarioEmisorId",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "UsuarioReceptorId",
                table: "Notificaciones");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_EmisorId",
                table: "Notificaciones",
                column: "EmisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_ReceptorId",
                table: "Notificaciones",
                column: "ReceptorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_Usuarios_EmisorId",
                table: "Notificaciones",
                column: "EmisorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_Usuarios_ReceptorId",
                table: "Notificaciones",
                column: "ReceptorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_Usuarios_EmisorId",
                table: "Notificaciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Notificaciones_Usuarios_ReceptorId",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_EmisorId",
                table: "Notificaciones");

            migrationBuilder.DropIndex(
                name: "IX_Notificaciones_ReceptorId",
                table: "Notificaciones");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioEmisorId",
                table: "Notificaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioReceptorId",
                table: "Notificaciones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_UsuarioEmisorId",
                table: "Notificaciones",
                column: "UsuarioEmisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificaciones_UsuarioReceptorId",
                table: "Notificaciones",
                column: "UsuarioReceptorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_Usuarios_UsuarioEmisorId",
                table: "Notificaciones",
                column: "UsuarioEmisorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notificaciones_Usuarios_UsuarioReceptorId",
                table: "Notificaciones",
                column: "UsuarioReceptorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
