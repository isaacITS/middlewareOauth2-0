using System;
using System.Collections.Generic;
using ITS_Middleware.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ITS_Middleware.Models.Context
{
    public partial class middlewareITSContext : DbContext
    {
        public middlewareITSContext()
        {
        }

        public middlewareITSContext(DbContextOptions<middlewareITSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MetodosAuth> MetodosAuths { get; set; } = null!;
        public virtual DbSet<Proyecto> Proyectos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<UsuariosProyecto> UsuariosProyectos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=it-seekersdev.ddns.net,65535; Database=ITS_OAuth; Uid=its-academy;  Pwd=Infinity01?; Encrypt=no;Connection Timeout=120; TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MetodosAuth>(entity =>
            {
                entity.ToTable("metodosAuth");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Proyecto>(entity =>
            {
                entity.ToTable("proyectos");

                entity.HasIndex(e => e.Nombre, "UQ__proyecto__72AFBCC613E2DAAA")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.Descripcion)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.FechaAlta)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaAlta");

                entity.Property(e => e.IdUsuarioRegsitra).HasColumnName("idUsuarioRegsitra");

                entity.Property(e => e.MetodosAutenticacion)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("metodosAutenticacion");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Enlace)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("enlace");

                entity.HasOne(d => d.IdUsuarioRegsitraNavigation)
                    .WithMany(p => p.Proyectos)
                    .HasForeignKey(d => d.IdUsuarioRegsitra)
                    .HasConstraintName("FK__proyectos__idUsu__06CD04F7");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");

                entity.HasIndex(e => e.Email, "UQ__usuarios__AB6E6164A123E0D5")
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

                entity.Property(e => e.TokenRecovery)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("tokenRecovery");
            });

            modelBuilder.Entity<UsuariosProyecto>(entity =>
            {
                entity.ToTable("usuariosProyecto");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Activo).HasColumnName("activo");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FechaAcceso)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaAcceso");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fechaCreacion");

                entity.Property(e => e.IdProyecto).HasColumnName("idProyecto");

                entity.Property(e => e.NombreCompleto)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("nombreCompleto");

                entity.Property(e => e.Pass)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("pass");

                entity.HasOne(d => d.IdProyectoNavigation)
                    .WithMany(p => p.UsuariosProyectos)
                    .HasForeignKey(d => d.IdProyecto)
                    .HasConstraintName("FK__usuariosP__idPro__09A971A2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
