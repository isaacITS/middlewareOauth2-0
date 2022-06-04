﻿using System;
using System.Collections.Generic;
using ITS_Middleware.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ITS_Middleware.Models
{
    public partial class MiddlewareDbContext : DbContext
    {
        public MiddlewareDbContext()
        {
        }

        public MiddlewareDbContext(DbContextOptions<MiddlewareDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Proyecto> Proyectos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=LENOVO-PC;Initial Catalog=middlewareITS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Proyecto>(entity =>
            {
                entity.ToTable("proyectos");

                entity.HasIndex(e => e.Nombre, "UQ__proyecto__72AFBCC6349FBD46")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(1)
                    .HasColumnName("descripcion");

                entity.Property(e => e.MetodoAutenticacion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("metodoAutenticacion");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Pass)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("pass");

                entity.Property(e => e.TipoCifrado)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tipoCifrado");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usuario");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");

                entity.HasIndex(e => e.Email, "UQ__usuarios__AB6E6164B23D24B9")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.Email)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FechaAlta)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaAlta");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Pass)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("pass");

                entity.Property(e => e.Puesto)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("puesto");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}