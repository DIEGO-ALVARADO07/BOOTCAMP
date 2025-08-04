using Dapper;
using Entity.Model.Base;
using Entity.Model.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq.Expressions;



namespace Entity.Context
{
    public class ApplicationDbContext : DbContext
    {

        protected readonly IConfiguration _configuration;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        
        //Dbset SETS
        public DbSet<User> Users { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<Departure> Departures { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Mazo> Mazos { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<ModeGame> ModeGames { get; set; }
        public DbSet<ImageEmoji> ImageEmojis { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasOne(d => d.Avatar)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Departure>()
                .HasMany(p => p.Users)
                .WithOne(c => c.Departure)
                .HasForeignKey(c => c.IdDeparture)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Departure>()
              .HasMany(p => p.Rounds)
              .WithOne(c => c.Departure)
              .HasForeignKey(c => c.IdDeparture)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Departure>()
              .HasMany(p => p.Mazos)
              .WithOne(c => c.Departure)
              .HasForeignKey(c => c.IdDeparture)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Departure>()
              .HasMany(p => p.ImageEmogis)
              .WithOne(c => c.Departure)
              .HasForeignKey(c => c.IdDeparture)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Departure>()
              .HasOne(p => p.ModeGame)
              .WithOne(c => c.Departure)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mazo>()
              .HasMany(p => p.Cards)
              .WithOne(c => c.Mazo)
              .HasForeignKey(c => c.IdMazo)
              .OnDelete(DeleteBehavior.Restrict);
        }



        /// <summary>
        /// Configura opciones adicionales del contexto, como el registro de datos sensibles.
        /// </summary>
        /// <param name="optionsBuilder">Constructor de opciones de configuración del contexto.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            // Otras configuraciones adicionales pueden ir aquí
        }

        /// <summary>
        /// Configura convenciones de tipos de datos, estableciendo la precisión por defecto de los valores decimales.
        /// </summary>
        /// <param name="configurationBuilder">Constructor de configuración de modelos.</param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        /// <summary>
        /// Guarda los cambios en la base de datos, asegurando la auditoría antes de persistir los datos.
        /// </summary>
        /// <returns>Número de filas afectadas.</returns>
        public override int SaveChanges()
        {
            EnsureAudit();
            return base.SaveChanges();
        }

        /// <summary>
        /// Guarda los cambios en la base de datos de manera asíncrona, asegurando la auditoría antes de la persistencia.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indica si se deben aceptar todos los cambios en caso de éxito.</param>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación.</param>
        /// <returns>Número de filas afectadas de forma asíncrona.</returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            EnsureAudit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Ejecuta una consulta SQL utilizando Dapper y devuelve una colección de resultados de tipo genérico.
        /// </summary>
        /// <typeparam name="T">Tipo de los datos de retorno.</typeparam>
        /// <param name="text">Consulta SQL a ejecutar.</param>
        /// <param name="parameters">Parámetros opcionales de la consulta.</param>
        /// <param name="timeout">Tiempo de espera opcional para la consulta.</param>
        /// <param name="type">Tipo opcional de comando SQL.</param>
        /// <returns>Una colección de objetos del tipo especificado.</returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string text, object? parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, text, parameters ?? new { }, timeout, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.QueryAsync<T>(command.Definition);
        }

        /// <summary>
        /// Método interno para garantizar la auditoría de los cambios en las entidades.
        /// </summary>
        private void EnsureAudit()
        {
            ChangeTracker.DetectChanges();
            
        }

        /// <summary>
        /// Estructura para ejecutar comandos SQL con Dapper en Entity Framework Core.
        /// </summary>
        public readonly struct DapperEFCoreCommand : IDisposable
        {
            /// <summary>
            /// Constructor del comando Dapper.
            /// </summary>
            /// <param name="context">Contexto de la base de datos.</param>
            /// <param name="text">Consulta SQL.</param>
            /// <param name="parameters">Parámetros opcionales.</param>
            /// <param name="timeout">Tiempo de espera opcional.</param>
            /// <param name="type">Tipo de comando SQL opcional.</param>
            /// <param name="ct">Token de cancelación.</param>
            public DapperEFCoreCommand(DbContext context, string text, object parameters, int? timeout, CommandType? type, CancellationToken ct)
            {
                var transaction = context.Database.CurrentTransaction?.GetDbTransaction();
                var commandType = type ?? CommandType.Text;
                var commandTimeout = timeout ?? context.Database.GetCommandTimeout() ?? 30;

                Definition = new CommandDefinition(
                    text,
                    parameters,
                    transaction,
                    commandTimeout,
                    commandType,
                    cancellationToken: ct
                );
            }

            //    /// <summary>
            //    /// Define los parámetros del comando SQL.
            //    /// </summary>
            public CommandDefinition Definition { get; }

            //    /// <summary>
            //    /// Método para liberar los recursos.
            //    /// </summary>
            public void Dispose()
            {
            }
        }
    }
}