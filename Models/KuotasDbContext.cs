using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KalkulosCore.Models;

public partial class KuotasDbContext : IdentityDbContext
{
    public KuotasDbContext(DbContextOptions<KuotasDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Acreedore> Acreedores { get; set; }

    public virtual DbSet<AuditoriaPrestamo> AuditoriaPrestamos { get; set; }

    public virtual DbSet<Calculadora> Calculadoras { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Cuota> Cuotas { get; set; }

    public virtual DbSet<CuotasBorrada> CuotasBorradas { get; set; }

    public virtual DbSet<CuotasRespaldo> CuotasRespaldos { get; set; }

    public virtual DbSet<EstadoSituacion> EstadoSituacions { get; set; }

    public virtual DbSet<HistorialAcceso> HistorialAccesos { get; set; }

    public virtual DbSet<HistorialPrestamo> HistorialPrestamos { get; set; }

    public virtual DbSet<Imprimircuota> Imprimircuotas { get; set; }

    public virtual DbSet<Imprimirliquidacion> Imprimirliquidacions { get; set; }

    public virtual DbSet<Manuale> Manuales { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<PagosBorrado> PagosBorrados { get; set; }

    public virtual DbSet<Pagosrespaldo> Pagosrespaldos { get; set; }

    public virtual DbSet<Pagostotal> Pagostotals { get; set; }

    public virtual DbSet<Pdfoperacion> Pdfoperacions { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    public virtual DbSet<PrestamoRespaldo> PrestamoRespaldos { get; set; }

    public virtual DbSet<PrestamosBorrado> PrestamosBorrados { get; set; }

    public virtual DbSet<Solicitude> Solicitudes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         base.OnModelCreating(modelBuilder);

        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Acreedore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ACREEDORES");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(80)
                .HasColumnName("APELLIDOS");
            entity.Property(e => e.Direccion)
                .HasMaxLength(150)
                .HasColumnName("DIRECCION");
            entity.Property(e => e.Documento).HasColumnName("DOCUMENTO");
            entity.Property(e => e.Nombres)
                .HasMaxLength(80)
                .HasColumnName("NOMBRES");
            entity.Property(e => e.Telcelular)
                .HasMaxLength(25)
                .HasColumnName("TELCELULAR");
            entity.Property(e => e.Telfijo)
                .HasMaxLength(25)
                .HasColumnName("TELFIJO");
        });

        modelBuilder.Entity<AuditoriaPrestamo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("AUDITORIA_PRESTAMOS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Accion)
                .HasMaxLength(50)
                .HasColumnName("ACCION");
            entity.Property(e => e.Modificacion)
                .HasColumnType("datetime")
                .HasColumnName("MODIFICACION");
            entity.Property(e => e.Operacion).HasColumnName("OPERACION");
            entity.Property(e => e.UsuarioId).HasColumnName("USUARIO_ID");
        });

        modelBuilder.Entity<Calculadora>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("calculadora");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Texto).HasColumnName("texto");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Idcliente).HasName("PRIMARY");

            entity.ToTable("CLIENTES");

            entity.Property(e => e.Idcliente).HasColumnName("IDCLIENTE");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .HasColumnName("DIRECCION");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Nombres)
                .HasMaxLength(200)
                .HasColumnName("NOMBRES");
            entity.Property(e => e.Telcelular)
                .HasMaxLength(25)
                .HasColumnName("TELCELULAR");
            entity.Property(e => e.Telfijo)
                .HasMaxLength(25)
                .HasColumnName("TELFIJO");
        });

        modelBuilder.Entity<Cuota>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("CUOTAS");

            // Estas son las propiedades de la tabla que ya estaban configuradas
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Capital).HasColumnName("CAPITAL");
            entity.Property(e => e.Estado).HasMaxLength(100);
            entity.Property(e => e.Idcliente).HasColumnName("IDCLIENTE");
            entity.Property(e => e.Moneda).HasMaxLength(20);
            entity.Property(e => e.Monto).HasColumnName("MONTO");
            entity.Property(e => e.Numero).HasColumnName("NUMERO");
            entity.Property(e => e.Numerodepago).HasColumnName("NUMERODEPAGO");
            entity.Property(e => e.Operacion).HasColumnName("OPERACION");
            entity.Property(e => e.Pago).HasColumnName("PAGO");
            entity.Property(e => e.Tipodecuotas).HasMaxLength(100);
            entity.Property(e => e.Tipooperacion).HasMaxLength(100);
            entity.Property(e => e.Vence).HasColumnType("datetime");

            // --- LÍNEA AÑADIDA PARA DEFINIR LA RELACIÓN ---
            // Le decimos que una Cuota tiene un Préstamo (a través de la propiedad OperacionNavigation)
            // y que la clave foránea en la tabla CUOTAS es la columna "Operacion".
            entity.HasOne(d => d.OperacionNavigation).WithMany(p => p.Cuota)
                .HasForeignKey(d => d.Operacion)
                .HasConstraintName("FK_Cuota_Prestamo"); // El nombre es opcional pero es una buena práctica
        });

        modelBuilder.Entity<CuotasBorrada>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CUOTAS_BORRADAS");

            entity.Property(e => e.Capital).HasColumnName("CAPITAL");
            entity.Property(e => e.Estado)
                .HasMaxLength(100)
                .HasColumnName("ESTADO");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idcliente).HasColumnName("IDCLIENTE");
            entity.Property(e => e.Moneda)
                .HasMaxLength(20)
                .HasColumnName("MONEDA");
            entity.Property(e => e.Monto).HasColumnName("MONTO");
            entity.Property(e => e.Numero).HasColumnName("NUMERO");
            entity.Property(e => e.Numerodepago).HasColumnName("NUMERODEPAGO");
            entity.Property(e => e.Operacion).HasColumnName("OPERACION");
            entity.Property(e => e.Pago).HasColumnName("PAGO");
            entity.Property(e => e.Tipodecuotas)
                .HasMaxLength(100)
                .HasColumnName("TIPODECUOTAS");
            entity.Property(e => e.Tipooperacion)
                .HasMaxLength(100)
                .HasColumnName("TIPOOPERACION");
            entity.Property(e => e.Vence)
                .HasColumnType("datetime")
                .HasColumnName("VENCE");
        });

        modelBuilder.Entity<CuotasRespaldo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("CUOTAS_RESPALDO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Capital).HasColumnName("CAPITAL");
            entity.Property(e => e.Estado)
                .HasMaxLength(100)
                .HasColumnName("ESTADO");
            entity.Property(e => e.Idcliente).HasColumnName("IDCLIENTE");
            entity.Property(e => e.Moneda)
                .HasMaxLength(20)
                .HasColumnName("MONEDA");
            entity.Property(e => e.Monto).HasColumnName("MONTO");
            entity.Property(e => e.Numero).HasColumnName("NUMERO");
            entity.Property(e => e.Numerodepago).HasColumnName("NUMERODEPAGO");
            entity.Property(e => e.Operacion).HasColumnName("OPERACION");
            entity.Property(e => e.Pago).HasColumnName("PAGO");
            entity.Property(e => e.Tipodecuotas)
                .HasMaxLength(100)
                .HasColumnName("TIPODECUOTAS");
            entity.Property(e => e.Tipooperacion)
                .HasMaxLength(100)
                .HasColumnName("TIPOOPERACION");
            entity.Property(e => e.Vence)
                .HasColumnType("datetime")
                .HasColumnName("VENCE");
        });

        modelBuilder.Entity<EstadoSituacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ESTADO_SITUACION");

            entity.Property(e => e.CashDolares).HasPrecision(18);
            entity.Property(e => e.CashPesos).HasPrecision(18);
            entity.Property(e => e.Concepto).HasMaxLength(300);
            entity.Property(e => e.MontoUi).HasColumnName("MontoUI");
            entity.Property(e => e.TipoDeCuotas).HasMaxLength(50);
        });

        modelBuilder.Entity<HistorialAcceso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("HISTORIAL_ACCESOS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Ingreso)
                .HasColumnType("datetime")
                .HasColumnName("INGRESO");
            entity.Property(e => e.Salida)
                .HasColumnType("datetime")
                .HasColumnName("SALIDA");
            entity.Property(e => e.UsuarioId).HasColumnName("USUARIO_ID");
        });

        modelBuilder.Entity<HistorialPrestamo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("HISTORIAL_PRESTAMOS");

            entity.Property(e => e.Capital).HasColumnName("CAPITAL");
            entity.Property(e => e.Capitalinicial).HasColumnName("CAPITALINICIAL");
            entity.Property(e => e.Carpeta)
                .HasMaxLength(100)
                .HasColumnName("CARPETA");
            entity.Property(e => e.Comentarios)
                .HasColumnType("text")
                .HasColumnName("COMENTARIOS");
            entity.Property(e => e.Cuotas).HasColumnName("CUOTAS");
            entity.Property(e => e.Dadorenprenda)
                .HasMaxLength(200)
                .HasColumnName("DADORENPRENDA");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("FECHA");
            entity.Property(e => e.Hipotecante)
                .HasMaxLength(200)
                .HasColumnName("HIPOTECANTE");
            entity.Property(e => e.Idacreedor).HasColumnName("IDACREEDOR");
            entity.Property(e => e.Idcliente).HasColumnName("IDCLIENTE");
            entity.Property(e => e.Lugardepago)
                .HasMaxLength(150)
                .HasColumnName("LUGARDEPAGO");
            entity.Property(e => e.Moneda)
                .HasMaxLength(20)
                .HasColumnName("MONEDA");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .HasColumnName("OBSERVACIONES");
            entity.Property(e => e.Operacion).HasColumnName("OPERACION");
            entity.Property(e => e.Padronhipotecario)
                .HasMaxLength(200)
                .HasColumnName("PADRONHIPOTECARIO");
            entity.Property(e => e.Primervencimiento)
                .HasMaxLength(30)
                .HasColumnName("PRIMERVENCIMIENTO");
            entity.Property(e => e.SysEndTime).HasMaxLength(6);
            entity.Property(e => e.SysStartTime).HasMaxLength(6);
            entity.Property(e => e.Tea).HasColumnName("TEA");
            entity.Property(e => e.Tem).HasColumnName("TEM");
            entity.Property(e => e.Tipodecuotas)
                .HasMaxLength(100)
                .HasColumnName("TIPODECUOTAS");
            entity.Property(e => e.Tipodevencimiento)
                .HasMaxLength(100)
                .HasColumnName("TIPODEVENCIMIENTO");
            entity.Property(e => e.Tipogarantia)
                .HasMaxLength(50)
                .HasColumnName("TIPOGARANTIA");
            entity.Property(e => e.Tipooperacion)
                .HasMaxLength(50)
                .HasColumnName("TIPOOPERACION");
            entity.Property(e => e.Tmora).HasColumnName("TMORA");
            entity.Property(e => e.Vehiculoprendado)
                .HasMaxLength(200)
                .HasColumnName("VEHICULOPRENDADO");
            entity.Property(e => e.Vencimientofinal)
                .HasMaxLength(2000)
                .HasColumnName("VENCIMIENTOFINAL");
        });

        modelBuilder.Entity<Imprimircuota>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("IMPRIMIRCUOTAS");

            entity.Property(e => e.Capital)
                .HasMaxLength(150)
                .HasColumnName("CAPITAL");
            entity.Property(e => e.Cuotaenpesos)
                .HasMaxLength(150)
                .HasColumnName("CUOTAENPESOS");
            entity.Property(e => e.Cuotasenui)
                .HasMaxLength(150)
                .HasColumnName("CUOTASENUI");
            entity.Property(e => e.Meses)
                .HasMaxLength(150)
                .HasColumnName("MESES");
        });

        modelBuilder.Entity<Imprimirliquidacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("IMPRIMIRLIQUIDACION");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Concepto)
                .HasMaxLength(500)
                .HasColumnName("CONCEPTO");
            entity.Property(e => e.Dias)
                .HasMaxLength(50)
                .HasColumnName("DIAS");
            entity.Property(e => e.Fechahasta)
                .HasMaxLength(50)
                .HasColumnName("FECHAHASTA");
            entity.Property(e => e.Fechainicio)
                .HasMaxLength(50)
                .HasColumnName("FECHAINICIO");
            entity.Property(e => e.Monto).HasColumnName("MONTO");
            entity.Property(e => e.Mora).HasColumnName("MORA");
            entity.Property(e => e.Total).HasColumnName("TOTAL");
        });

        modelBuilder.Entity<Manuale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("MANUALES");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Archivo).HasColumnName("ARCHIVO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("PAGOS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("FECHA");
            entity.Property(e => e.Idcliente).HasColumnName("IDCLIENTE");
            entity.Property(e => e.Idcuota).HasColumnName("IDCUOTA");
            entity.Property(e => e.Monto).HasColumnName("MONTO");
            entity.Property(e => e.Numero).HasColumnName("NUMERO");
            entity.Property(e => e.Operacion).HasColumnName("OPERACION");
            entity.Property(e => e.Tipo)
                .HasMaxLength(100)
                .HasColumnName("TIPO");
            entity.Property(e => e.Tipocuota)
                .HasMaxLength(100)
                .HasColumnName("TIPOCUOTA");
        });

        modelBuilder.Entity<PagosBorrado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("PAGOS_BORRADOS");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("FECHA");
            entity.Property(e => e.Idcliente).HasColumnName("IDCLIENTE");
            entity.Property(e => e.Idcuota).HasColumnName("IDCUOTA");
            entity.Property(e => e.Monto).HasColumnName("MONTO");
            entity.Property(e => e.Numero).HasColumnName("NUMERO");
            entity.Property(e => e.Operacion).HasColumnName("OPERACION");
            entity.Property(e => e.Tipo)
                .HasMaxLength(100)
                .HasColumnName("TIPO");
            entity.Property(e => e.Tipocuota)
                .HasMaxLength(100)
                .HasColumnName("TIPOCUOTA");
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasKey(e => e.Operacion).HasName("PRIMARY");

            entity.ToTable("PRESTAMO");

            entity.Property(e => e.Operacion).HasColumnName("OPERACION");
            entity.Property(e => e.Capital).HasColumnName("CAPITAL");
            entity.Property(e => e.Capitalinicial).HasColumnName("CAPITALINICIAL");
            entity.Property(e => e.Carpeta).HasMaxLength(100);
            entity.Property(e => e.Comentarios).HasColumnType("text");
            entity.Property(e => e.Cuotas).HasColumnName("CUOTAS");
            entity.Property(e => e.Dadorenprenda).HasMaxLength(200).HasColumnName("DADORENPRENDA");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Hipotecante).HasMaxLength(200);
            entity.Property(e => e.Idacreedor).HasColumnName("IDACREEDOR");
            entity.Property(e => e.Idcliente).HasColumnName("IDCLIENTE");
            entity.Property(e => e.Lugardepago).HasMaxLength(150);
            entity.Property(e => e.Moneda).HasMaxLength(20);
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.Padronhipotecario).HasMaxLength(200);
            entity.Property(e => e.Primervencimiento).HasMaxLength(30);
            entity.Property(e => e.SysEndTime).HasMaxLength(6).HasDefaultValueSql("'9999-12-31 23:59:59.999999'");
            entity.Property(e => e.SysStartTime).HasMaxLength(6).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            entity.Property(e => e.Tea).HasColumnName("TEA");
            entity.Property(e => e.Tem).HasColumnName("TEM");
            entity.Property(e => e.Tipodecuotas).HasMaxLength(100);
            entity.Property(e => e.Tipodevencimiento).HasMaxLength(100);
            entity.Property(e => e.Tipogarantia).HasMaxLength(50);
            entity.Property(e => e.Tipooperacion).HasMaxLength(50);
            entity.Property(e => e.Tmora).HasColumnName("TMORA");
            entity.Property(e => e.Vehiculoprendado).HasMaxLength(200);
            entity.Property(e => e.Vencimientofinal).HasMaxLength(2000);

            // --- LÍNEAS AÑADIDAS PARA DEFINIR LA RELACIÓN ---
            entity.HasOne(d => d.IdacreedorNavigation).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.Idacreedor)
                .HasConstraintName("FK_Prestamo_Acreedor"); // El nombre de la constraint es opcional

            entity.HasOne(d => d.IdclienteNavigation).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.Idcliente)
                .HasConstraintName("FK_Prestamo_Cliente"); // El nombre de la constraint es opcional
        });

        modelBuilder.Entity<Pagostotal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("PAGOSTOTAL");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("FECHA");
            entity.Property(e => e.Idcliente).HasColumnName("IDCLIENTE");
            entity.Property(e => e.Monto).HasColumnName("MONTO");
            entity.Property(e => e.NombreDeudor)
                .HasMaxLength(100)
                .HasColumnName("NOMBRE_DEUDOR");
        });

        modelBuilder.Entity<Pdfoperacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("PDFOPERACION");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Archivo).HasColumnName("ARCHIVO");
            entity.Property(e => e.Idcliente).HasColumnName("IDCLIENTE");
            entity.Property(e => e.Idoperacion).HasColumnName("IDOPERACION");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasKey(e => e.Operacion).HasName("PRIMARY");

            entity.ToTable("PRESTAMO");

            entity.Property(e => e.Operacion).HasColumnName("OPERACION");
            entity.Property(e => e.Capital).HasColumnName("CAPITAL");
            entity.Property(e => e.Capitalinicial).HasColumnName("CAPITALINICIAL");
            entity.Property(e => e.Carpeta)
                .HasMaxLength(100)
                .HasColumnName("CARPETA");
            entity.Property(e => e.Comentarios)
                .HasColumnType("text")
                .HasColumnName("COMENTARIOS");
            entity.Property(e => e.Cuotas).HasColumnName("CUOTAS");
            entity.Property(e => e.Dadorenprenda)
                .HasMaxLength(200)
                .HasColumnName("DADORENPRENDA");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("FECHA");
            entity.Property(e => e.Hipotecante)
                .HasMaxLength(200)
                .HasColumnName("HIPOTECANTE");
            entity.Property(e => e.Idacreedor).HasColumnName("IDACREEDOR");
            entity.Property(e => e.Idcliente).HasColumnName("IDCLIENTE");
            entity.Property(e => e.Lugardepago)
                .HasMaxLength(150)
                .HasColumnName("LUGARDEPAGO");
            entity.Property(e => e.Moneda)
                .HasMaxLength(20)
                .HasColumnName("MONEDA");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .HasColumnName("OBSERVACIONES");
            entity.Property(e => e.Padronhipotecario)
                .HasMaxLength(200)
                .HasColumnName("PADRONHIPOTECARIO");
            entity.Property(e => e.Primervencimiento)
                .HasMaxLength(30)
                .HasColumnName("PRIMERVENCIMIENTO");
            entity.Property(e => e.SysEndTime)
                .HasMaxLength(6)
                .HasDefaultValueSql("'9999-12-31 23:59:59.999999'");
            entity.Property(e => e.SysStartTime)
                .HasMaxLength(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            entity.Property(e => e.Tea).HasColumnName("TEA");
            entity.Property(e => e.Tem).HasColumnName("TEM");
            entity.Property(e => e.Tipodecuotas)
                .HasMaxLength(100)
                .HasColumnName("TIPODECUOTAS");
            entity.Property(e => e.Tipodevencimiento)
                .HasMaxLength(100)
                .HasColumnName("TIPODEVENCIMIENTO");
            entity.Property(e => e.Tipogarantia)
                .HasMaxLength(50)
                .HasColumnName("TIPOGARANTIA");
            entity.Property(e => e.Tipooperacion)
                .HasMaxLength(50)
                .HasColumnName("TIPOOPERACION");
            entity.Property(e => e.Tmora).HasColumnName("TMORA");
            entity.Property(e => e.Vehiculoprendado)
                .HasMaxLength(200)
                .HasColumnName("VEHICULOPRENDADO");
            entity.Property(e => e.Vencimientofinal)
                .HasMaxLength(2000)
                .HasColumnName("VENCIMIENTOFINAL");
        });

        modelBuilder.Entity<PrestamoRespaldo>(entity =>
        {
            entity.HasKey(e => e.Operacion).HasName("PRIMARY");

            entity.ToTable("PRESTAMO_RESPALDO");

            entity.Property(e => e.Operacion).HasColumnName("OPERACION");
            entity.Property(e => e.Capital).HasColumnName("CAPITAL");
            entity.Property(e => e.Capitalinicial).HasColumnName("CAPITALINICIAL");
            entity.Property(e => e.Carpeta)
                .HasMaxLength(100)
                .HasColumnName("CARPETA");
            entity.Property(e => e.Comentarios)
                .HasColumnType("text")
                .HasColumnName("COMENTARIOS");
            entity.Property(e => e.Cuotas).HasColumnName("CUOTAS");
            entity.Property(e => e.Dadorenprenda)
                .HasMaxLength(200)
                .HasColumnName("DADORENPRENDA");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("FECHA");
            entity.Property(e => e.Hipotecante)
                .HasMaxLength(200)
                .HasColumnName("HIPOTECANTE");
            entity.Property(e => e.Idacreedor).HasColumnName("IDACREEDOR");
            entity.Property(e => e.Idcliente).HasColumnName("IDCLIENTE");
            entity.Property(e => e.Lugardepago)
                .HasMaxLength(150)
                .HasColumnName("LUGARDEPAGO");
            entity.Property(e => e.Moneda)
                .HasMaxLength(20)
                .HasColumnName("MONEDA");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .HasColumnName("OBSERVACIONES");
            entity.Property(e => e.Padronhipotecario)
                .HasMaxLength(200)
                .HasColumnName("PADRONHIPOTECARIO");
            entity.Property(e => e.Primervencimiento)
                .HasMaxLength(30)
                .HasColumnName("PRIMERVENCIMIENTO");
            entity.Property(e => e.Tea).HasColumnName("TEA");
            entity.Property(e => e.Tem).HasColumnName("TEM");
            entity.Property(e => e.Tipodecuotas)
                .HasMaxLength(100)
                .HasColumnName("TIPODECUOTAS");
            entity.Property(e => e.Tipodevencimiento)
                .HasMaxLength(100)
                .HasColumnName("TIPODEVENCIMIENTO");
            entity.Property(e => e.Tipogarantia)
                .HasMaxLength(50)
                .HasColumnName("TIPOGARANTIA");
            entity.Property(e => e.Tipooperacion)
                .HasMaxLength(50)
                .HasColumnName("TIPOOPERACION");
            entity.Property(e => e.Tmora).HasColumnName("TMORA");
            entity.Property(e => e.Vehiculoprendado)
                .HasMaxLength(200)
                .HasColumnName("VEHICULOPRENDADO");
            entity.Property(e => e.Vencimientofinal)
                .HasMaxLength(2000)
                .HasColumnName("VENCIMIENTOFINAL");
        });

        modelBuilder.Entity<PrestamosBorrado>(entity =>
        {
            entity.HasKey(e => e.Operacion).HasName("PRIMARY");

            entity.ToTable("PRESTAMOS_BORRADOS");

            entity.Property(e => e.Operacion)
                .ValueGeneratedNever()
                .HasColumnName("OPERACION");
            entity.Property(e => e.Capital).HasColumnName("CAPITAL");
            entity.Property(e => e.Capitalinicial).HasColumnName("CAPITALINICIAL");
            entity.Property(e => e.Carpeta)
                .HasMaxLength(100)
                .HasColumnName("CARPETA");
            entity.Property(e => e.Comentarios)
                .HasColumnType("text")
                .HasColumnName("COMENTARIOS");
            entity.Property(e => e.Cuotas).HasColumnName("CUOTAS");
            entity.Property(e => e.Dadorenprenda)
                .HasMaxLength(200)
                .HasColumnName("DADORENPRENDA");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("FECHA");
            entity.Property(e => e.Hipotecante)
                .HasMaxLength(200)
                .HasColumnName("HIPOTECANTE");
            entity.Property(e => e.Idacreedor).HasColumnName("IDACREEDOR");
            entity.Property(e => e.Idcliente).HasColumnName("IDCLIENTE");
            entity.Property(e => e.Lugardepago)
                .HasMaxLength(150)
                .HasColumnName("LUGARDEPAGO");
            entity.Property(e => e.Moneda)
                .HasMaxLength(50)
                .HasColumnName("MONEDA");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .HasColumnName("OBSERVACIONES");
            entity.Property(e => e.Padronhipotecario)
                .HasMaxLength(200)
                .HasColumnName("PADRONHIPOTECARIO");
            entity.Property(e => e.Primervencimiento)
                .HasMaxLength(30)
                .HasColumnName("PRIMERVENCIMIENTO");
            entity.Property(e => e.Tea).HasColumnName("TEA");
            entity.Property(e => e.Tem).HasColumnName("TEM");
            entity.Property(e => e.Tipodecuotas)
                .HasMaxLength(50)
                .HasColumnName("TIPODECUOTAS");
            entity.Property(e => e.Tipodevencimiento)
                .HasMaxLength(100)
                .HasColumnName("TIPODEVENCIMIENTO");
            entity.Property(e => e.Tipogarantia)
                .HasMaxLength(50)
                .HasColumnName("TIPOGARANTIA");
            entity.Property(e => e.Tipooperacion)
                .HasMaxLength(50)
                .HasColumnName("TIPOOPERACION");
            entity.Property(e => e.Tmora).HasColumnName("TMORA");
            entity.Property(e => e.Vehiculoprendado)
                .HasMaxLength(200)
                .HasColumnName("VEHICULOPRENDADO");
            entity.Property(e => e.Vencimientofinal)
                .HasMaxLength(2000)
                .HasColumnName("VENCIMIENTOFINAL");
        });

        modelBuilder.Entity<Solicitude>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("SOLICITUDES");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Estado)
                .HasMaxLength(200)
                .HasColumnName("ESTADO");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("FECHA");
            entity.Property(e => e.Garantia)
                .HasColumnType("text")
                .HasColumnName("GARANTIA");
            entity.Property(e => e.Monto)
                .HasMaxLength(200)
                .HasColumnName("MONTO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(300)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Telefono)
                .HasMaxLength(200)
                .HasColumnName("TELEFONO");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("USUARIO");

            entity.HasIndex(e => e.Usuario1, "UQ_USUARIO_USUARIO").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(50)
                .HasColumnName("CONTRASEÑA");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .HasColumnName("TIPO");
            entity.Property(e => e.Usuario1)
                .HasMaxLength(50)
                .HasColumnName("USUARIO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
