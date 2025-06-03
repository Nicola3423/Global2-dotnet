using Microsoft.EntityFrameworkCore;
using Sessions_app.Models;
using Microsoft.Extensions.Configuration;
using Oracle.EntityFrameworkCore;

namespace Sessions_app.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<RotaSegura> RotaSeguras { get; set; }
        public DbSet<Risco> Riscos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseOracle(
                    configuration.GetConnectionString("OracleConnection")
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração do schema (seu usuário Oracle)
            modelBuilder.HasDefaultSchema("RM553991");

            modelBuilder.Ignore<Link>();

            // Configuração da entidade Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("USUARIO");
                entity.HasKey(e => e.Id).HasName("PK_USUARIO");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("NOME")
                    .HasColumnType("VARCHAR2(100)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("EMAIL")
                    .HasColumnType("VARCHAR2(100)");
            });

            // Configuração da entidade RotaSegura
            modelBuilder.Entity<RotaSegura>(entity =>
            {
                entity.ToTable("ROTA_SEGURA");
                entity.HasKey(e => e.Id).HasName("PK_ROTA_SEGURA");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Localizacao)
                    .IsRequired()
                    .HasColumnName("LOCALIZACAO")
                    .HasColumnType("VARCHAR2(200)");

                entity.Property(e => e.Coordenadas)
                    .HasColumnName("COORDENADAS")
                    .HasColumnType("VARCHAR2(100)");
            });

            // Configuração da entidade Risco
            modelBuilder.Entity<Risco>(entity =>
            {
                entity.ToTable("RISCO");
                entity.HasKey(e => e.Id).HasName("PK_RISCO");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasColumnName("DESCRICAO")
                    .HasColumnType("VARCHAR2(500)");

                entity.Property(e => e.Nivel)
                    .IsRequired()
                    .HasColumnName("NIVEL")
                    .HasColumnType("NUMBER(2)");

                // Configuração da restrição CHECK para o nível
                entity.HasCheckConstraint("CK_RISCO_NIVEL", "NIVEL BETWEEN 1 AND 5");
            });

            // Configuração de sequências para IDs
            modelBuilder.HasSequence<int>("USUARIO_SEQ")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.HasSequence<int>("ROTASEGURA_SEQ")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.HasSequence<int>("RISCO_SEQ")
                .StartsAt(1)
                .IncrementsBy(1);

            // Configuração de valores padrão para as sequências
            modelBuilder.Entity<Usuario>()
                .Property(e => e.Id)
                .HasDefaultValueSql("\"RM553991\".\"USUARIO_SEQ\".NEXTVAL");

            modelBuilder.Entity<RotaSegura>()
                .Property(e => e.Id)
                .HasDefaultValueSql("\"RM553991\".\"ROTASEGURA_SEQ\".NEXTVAL");

            modelBuilder.Entity<Risco>()
                .Property(e => e.Id)
                .HasDefaultValueSql("\"RM553991\".\"RISCO_SEQ\".NEXTVAL");
        }
    }
}