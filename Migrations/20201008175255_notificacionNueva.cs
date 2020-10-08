using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auriculoterapia.Api.Migrations
{
    public partial class notificacionNueva : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaNotificación",
                table: "Notificaciones");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNotificacion",
                table: "Notificaciones",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaNotificacion",
                table: "Notificaciones");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNotificación",
                table: "Notificaciones",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
