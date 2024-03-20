using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LemonAPI.Models
{
    public partial class dblemonContext : DbContext
    {
        public dblemonContext()
        {
        }

        public dblemonContext(DbContextOptions<dblemonContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Compra> Compras { get; set; } = null!;
        public virtual DbSet<Detallecompra> Detallecompras { get; set; } = null!;
        public virtual DbSet<Detalleventum> Detalleventa { get; set; } = null!;
        public virtual DbSet<Movimiento> Movimientos { get; set; } = null!;
        public virtual DbSet<Permiso> Permisos { get; set; } = null!;
        public virtual DbSet<Producto> Productos { get; set; } = null!;
        public virtual DbSet<Proveedor> Proveedors { get; set; } = null!;
        public virtual DbSet<Rol> Rols { get; set; } = null!;
        public virtual DbSet<Rolpermiso> Rolpermisos { get; set; } = null!;
        public virtual DbSet<Tipomovimiento> Tipomovimientos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<Ventum> Venta { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=password;database=dblemon");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente)
                    .HasName("PRIMARY");

                entity.ToTable("cliente");

                entity.HasIndex(e => e.Correo, "correo")
                    .IsUnique();

                entity.HasIndex(e => e.Documento, "documento")
                    .IsUnique();

                entity.HasIndex(e => e.Telefono, "telefono")
                    .IsUnique();

                entity.Property(e => e.IdCliente).HasColumnName("idCliente");

                entity.Property(e => e.Correo)
                    .HasMaxLength(50)
                    .HasColumnName("correo");

                entity.Property(e => e.Documento).HasColumnName("documento");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.NombreRazonSocial)
                    .HasMaxLength(30)
                    .HasColumnName("nombreRazonSocial");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(10)
                    .HasColumnName("telefono");

                entity.Property(e => e.TipoDocumento)
                    .HasMaxLength(30)
                    .HasColumnName("tipoDocumento");
            });

            modelBuilder.Entity<Compra>(entity =>
            {
                entity.HasKey(e => e.IdCompra)
                    .HasName("PRIMARY");

                entity.ToTable("compra");

                entity.HasIndex(e => e.IdProveedor, "idProveedor");

                entity.Property(e => e.IdCompra).HasColumnName("idCompra");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");

                entity.Property(e => e.ValorCompra).HasColumnName("valorCompra");

                entity.HasOne(d => d.IdProveedorNavigation)
                    .WithMany(p => p.Compras)
                    .HasForeignKey(d => d.IdProveedor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("compra_ibfk_1");
            });

            modelBuilder.Entity<Detallecompra>(entity =>
            {
                entity.HasKey(e => e.IdDetalleCompra)
                    .HasName("PRIMARY");

                entity.ToTable("detallecompra");

                entity.HasIndex(e => e.IdCompra, "idCompra");

                entity.HasIndex(e => e.IdProducto, "idProducto");

                entity.Property(e => e.IdDetalleCompra).HasColumnName("idDetalleCompra");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.IdCompra).HasColumnName("idCompra");

                entity.Property(e => e.IdProducto).HasColumnName("idProducto");

                entity.Property(e => e.PrecioKilo).HasColumnName("precioKilo");

                entity.Property(e => e.Subtotal).HasColumnName("subtotal");

                entity.HasOne(d => d.IdCompraNavigation)
                    .WithMany(p => p.Detallecompras)
                    .HasForeignKey(d => d.IdCompra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("detallecompra_ibfk_1");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.Detallecompras)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("detallecompra_ibfk_2");
            });

            modelBuilder.Entity<Detalleventum>(entity =>
            {
                entity.HasKey(e => e.IdDetalleVenta)
                    .HasName("PRIMARY");

                entity.ToTable("detalleventa");

                entity.HasIndex(e => e.IdProducto, "idProducto");

                entity.HasIndex(e => e.IdVenta, "idVenta");

                entity.Property(e => e.IdDetalleVenta).HasColumnName("idDetalleVenta");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.IdProducto).HasColumnName("idProducto");

                entity.Property(e => e.IdVenta).HasColumnName("idVenta");

                entity.Property(e => e.PrecioKilo).HasColumnName("precioKilo");

                entity.Property(e => e.Subtotal).HasColumnName("subtotal");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.Detalleventa)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("detalleventa_ibfk_2");

                entity.HasOne(d => d.IdVentaNavigation)
                    .WithMany(p => p.Detalleventa)
                    .HasForeignKey(d => d.IdVenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("detalleventa_ibfk_1");
            });

            modelBuilder.Entity<Movimiento>(entity =>
            {
                entity.HasKey(e => e.IdMovimiento)
                    .HasName("PRIMARY");

                entity.ToTable("movimiento");

                entity.HasIndex(e => e.IdProducto, "idProducto");

                entity.HasIndex(e => e.IdTipoMovimiento, "idTipoMovimiento");

                entity.Property(e => e.IdMovimiento).HasColumnName("idMovimiento");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdProducto).HasColumnName("idProducto");

                entity.Property(e => e.IdTipoMovimiento).HasColumnName("idTipoMovimiento");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.Movimientos)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("movimiento_ibfk_2");

                entity.HasOne(d => d.IdTipoMovimientoNavigation)
                    .WithMany(p => p.Movimientos)
                    .HasForeignKey(d => d.IdTipoMovimiento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("movimiento_ibfk_1");
            });

            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.HasKey(e => e.IdPermiso)
                    .HasName("PRIMARY");

                entity.ToTable("permiso");

                entity.Property(e => e.IdPermiso).HasColumnName("idPermiso");

                entity.Property(e => e.Permiso1)
                    .HasMaxLength(20)
                    .HasColumnName("permiso");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdProducto)
                    .HasName("PRIMARY");

                entity.ToTable("producto");

                entity.Property(e => e.IdProducto).HasColumnName("idProducto");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.Costo).HasColumnName("costo");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(30)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.HasKey(e => e.IdProveedor)
                    .HasName("PRIMARY");

                entity.ToTable("proveedor");

                entity.HasIndex(e => e.Correo, "correo")
                    .IsUnique();

                entity.HasIndex(e => e.Documento, "documento")
                    .IsUnique();

                entity.HasIndex(e => e.Telefono, "telefono")
                    .IsUnique();

                entity.Property(e => e.IdProveedor).HasColumnName("idProveedor");

                entity.Property(e => e.Correo)
                    .HasMaxLength(50)
                    .HasColumnName("correo");

                entity.Property(e => e.Documento).HasColumnName("documento");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.NombreRazonSocial)
                    .HasMaxLength(30)
                    .HasColumnName("nombreRazonSocial");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(10)
                    .HasColumnName("telefono");

                entity.Property(e => e.TipoDocumento)
                    .HasMaxLength(30)
                    .HasColumnName("tipoDocumento");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PRIMARY");

                entity.ToTable("rol");

                entity.Property(e => e.IdRol).HasColumnName("idRol");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(20)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Rolpermiso>(entity =>
            {
                entity.HasKey(e => e.IdRolPermiso)
                    .HasName("PRIMARY");

                entity.ToTable("rolpermiso");

                entity.HasIndex(e => e.IdPermiso, "idPermiso");

                entity.HasIndex(e => e.IdRol, "idRol");

                entity.Property(e => e.IdRolPermiso).HasColumnName("idRolPermiso");

                entity.Property(e => e.IdPermiso).HasColumnName("idPermiso");

                entity.Property(e => e.IdRol).HasColumnName("idRol");

                entity.HasOne(d => d.IdPermisoNavigation)
                    .WithMany(p => p.Rolpermisos)
                    .HasForeignKey(d => d.IdPermiso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rolpermiso_ibfk_2");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Rolpermisos)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rolpermiso_ibfk_1");
            });

            modelBuilder.Entity<Tipomovimiento>(entity =>
            {
                entity.HasKey(e => e.IdTipoMovimiento)
                    .HasName("PRIMARY");

                entity.ToTable("tipomovimiento");

                entity.Property(e => e.IdTipoMovimiento).HasColumnName("idTipoMovimiento");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(20)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PRIMARY");

                entity.ToTable("usuario");

                entity.HasIndex(e => e.Correo, "correo")
                    .IsUnique();

                entity.HasIndex(e => e.IdRol, "idRol");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Contraseña)
                    .HasMaxLength(15)
                    .HasColumnName("contraseña");

                entity.Property(e => e.Correo)
                    .HasMaxLength(40)
                    .HasColumnName("correo");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.IdRol).HasColumnName("idRol");

                entity.Property(e => e.NombreUsuario)
                    .HasMaxLength(30)
                    .HasColumnName("nombreUsuario");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("usuario_ibfk_1");
            });

            modelBuilder.Entity<Ventum>(entity =>
            {
                entity.HasKey(e => e.IdVenta)
                    .HasName("PRIMARY");

                entity.ToTable("venta");

                entity.HasIndex(e => e.IdCliente, "idCliente");

                entity.Property(e => e.IdVenta).HasColumnName("idVenta");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdCliente).HasColumnName("idCliente");

                entity.Property(e => e.Total).HasColumnName("total");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Venta)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("venta_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
