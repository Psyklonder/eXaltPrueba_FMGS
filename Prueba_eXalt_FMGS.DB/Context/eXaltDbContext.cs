using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Prueba_eXalt_FMGS.DB.Entities;

namespace Prueba_eXalt_FMGS.DB.Context
{
    public partial class eXaltDbContext : DbContext
    {
        public eXaltDbContext()
        {
        }

        public eXaltDbContext(DbContextOptions<eXaltDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Localidad> Localidad { get; set; } = null!;
        public virtual DbSet<Pedido> Pedido { get; set; } = null!;
        public virtual DbSet<PedidoDetalle> PedidoDetalle { get; set; } = null!;
        public virtual DbSet<PedidoEstado> PedidoEstado { get; set; } = null!;
        public virtual DbSet<Persona> Persona { get; set; } = null!;
        public virtual DbSet<Producto> Producto { get; set; } = null!;
        public virtual DbSet<ProductoTipo> ProductoTipo { get; set; } = null!;
        public virtual DbSet<Rol> Rol { get; set; } = null!;
        public virtual DbSet<Usuario> Usuario { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Localidad>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CodigoPostal)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.LocalidadPadre)
                    .WithMany(p => p.InverseLocalidadPadre)
                    .HasForeignKey(d => d.LocalidadPadreId)
                    .HasConstraintName("FK_Localidad_Localidad");
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descuento).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.NumeroFactura).ValueGeneratedOnAdd();

                entity.HasOne(d => d.PedidoEstado)
                    .WithMany(p => p.Pedido)
                    .HasForeignKey(d => d.PedidoEstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pedido_PedidoEstado");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Pedido)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pedido_Usuario");
            });

            modelBuilder.Entity<PedidoDetalle>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ValorPorUnidad).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Pedido)
                    .WithMany(p => p.PedidoDetalle)
                    .HasForeignKey(d => d.PedidoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PedidoDetalle_Pedido");

                entity.HasOne(d => d.Producto)
                    .WithMany(p => p.PedidoDetalle)
                    .HasForeignKey(d => d.ProductoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PedidoDetalle_Producto");
            });

            modelBuilder.Entity<PedidoEstado>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaNacimiento).HasColumnType("date");

                entity.Property(e => e.Nombres)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Localidad)
                    .WithMany(p => p.Persona)
                    .HasForeignKey(d => d.LocalidadId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Persona_Localidad");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descripcion).IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Precio).HasColumnType("decimal(18, 0)");

                entity.HasMany(d => d.ProductoTipo)
                    .WithMany(p => p.Producto)
                    .UsingEntity<Dictionary<string, object>>(
                        "Productos_Tipos",
                        l => l.HasOne<ProductoTipo>().WithMany().HasForeignKey("ProductoTipoId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Productos_Tipos_ProductoTipo"),
                        r => r.HasOne<Producto>().WithMany().HasForeignKey("ProductoId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Productos_Tipos_Producto"),
                        j =>
                        {
                            j.HasKey("ProductoId", "ProductoTipoId");

                            j.ToTable("Productos_Tipos");
                        });
            });

            modelBuilder.Entity<ProductoTipo>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Codigo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.PersonaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_Persona");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.RolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_Rol");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
