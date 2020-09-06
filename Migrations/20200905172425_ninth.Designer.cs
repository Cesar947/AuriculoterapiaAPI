﻿// <auto-generated />
using System;
using Auriculoterapia.Api.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Auriculoterapia.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200905172425_ninth")]
    partial class ninth
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Auriculoterapia.Api.Domain.Cita", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Estado")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("HoraFinAtencion")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("HoraInicioAtencion")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("PacienteId")
                        .HasColumnType("int");

                    b.Property<int>("TipoAtencionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PacienteId");

                    b.HasIndex("TipoAtencionId");

                    b.ToTable("Citas");
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.Disponibilidad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Dia")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EspecialistaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("HoraFin")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("HoraInicio")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("EspecialistaId");

                    b.ToTable("Disponibilidades");
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.Especialista", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AnhoExperiencia")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId")
                        .IsUnique();

                    b.ToTable("Especialistas");
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.HorarioDescartado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DisponibilidadId")
                        .HasColumnType("int");

                    b.Property<DateTime>("HoraFin")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("HoraInicio")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("DisponibilidadId");

                    b.ToTable("HorariosDescartados");
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.Paciente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Celular")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId")
                        .IsUnique();

                    b.ToTable("Pacientes");
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.Rol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.Rol_Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("RolId")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RolId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Rol_Usuarios");
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.SolicitudTratamiento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<float>("Altura")
                        .HasColumnType("float");

                    b.Property<int>("Edad")
                        .HasColumnType("int");

                    b.Property<string>("Estado")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ImagenAreaAfectada")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("OtrosSintomas")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("PacienteId")
                        .HasColumnType("int");

                    b.Property<float>("Peso")
                        .HasColumnType("float");

                    b.Property<string>("Sintomas")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("fechaInicio")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("PacienteId");

                    b.ToTable("SolicitudTratamientos");
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.TipoAtencion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("TipoAtencions");
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.Tratamiento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaEnvio")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("FrecuenciaAlDia")
                        .HasColumnType("int");

                    b.Property<string>("ImagenEditada")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("SolicitudTratamientoId")
                        .HasColumnType("int");

                    b.Property<int>("TiempoPorTerapia")
                        .HasColumnType("int");

                    b.Property<string>("TipoTratamiento")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("SolicitudTratamientoId");

                    b.ToTable("Tratamientos");
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Apellido")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Contrasena")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Nombre")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NombreUsuario")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PalabraClave")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Sexo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Token")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.Cita", b =>
                {
                    b.HasOne("Auriculoterapia.Api.Domain.Paciente", "Paciente")
                        .WithMany()
                        .HasForeignKey("PacienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Auriculoterapia.Api.Domain.TipoAtencion", "TipoAtencion")
                        .WithMany()
                        .HasForeignKey("TipoAtencionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.Disponibilidad", b =>
                {
                    b.HasOne("Auriculoterapia.Api.Domain.Especialista", "Especialista")
                        .WithMany()
                        .HasForeignKey("EspecialistaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.Especialista", b =>
                {
                    b.HasOne("Auriculoterapia.Api.Domain.Usuario", "Usuario")
                        .WithOne("Especialista")
                        .HasForeignKey("Auriculoterapia.Api.Domain.Especialista", "UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.HorarioDescartado", b =>
                {
                    b.HasOne("Auriculoterapia.Api.Domain.Disponibilidad", "Disponibilidad")
                        .WithMany("HorariosDescartados")
                        .HasForeignKey("DisponibilidadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.Paciente", b =>
                {
                    b.HasOne("Auriculoterapia.Api.Domain.Usuario", "Usuario")
                        .WithOne("Paciente")
                        .HasForeignKey("Auriculoterapia.Api.Domain.Paciente", "UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.Rol_Usuario", b =>
                {
                    b.HasOne("Auriculoterapia.Api.Domain.Rol", "Rol")
                        .WithMany()
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Auriculoterapia.Api.Domain.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.SolicitudTratamiento", b =>
                {
                    b.HasOne("Auriculoterapia.Api.Domain.Paciente", "Paciente")
                        .WithMany()
                        .HasForeignKey("PacienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Auriculoterapia.Api.Domain.Tratamiento", b =>
                {
                    b.HasOne("Auriculoterapia.Api.Domain.SolicitudTratamiento", "SolicitudTratamiento")
                        .WithMany()
                        .HasForeignKey("SolicitudTratamientoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
