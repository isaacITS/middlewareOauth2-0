using API_MIDDLEWARE.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace API_MIDDLEWARE.Models.Context
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

       
        public virtual DbSet<Usuarios> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=LENOVO-PC;Initial Catalog=middlewareITS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Usuarios>(entity =>
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

              /*  entity.Property(e => e.TokenRecovery)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("tokenRecovery");*/
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder); 
    }
}
