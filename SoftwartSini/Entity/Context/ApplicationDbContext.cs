using Dapper;
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de enum para convertir a string
            modelBuilder.Entity<Departure>()
                .Property(n => n.ModeGame)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
               .HasMany(d => d.Avatar)
               .WithOne(c => c.User)
               .HasForeignKey(d => d.IdUser)
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

            modelBuilder.Entity<Mazo>()
              .HasMany(p => p.Cards)
              .WithOne(c => c.Mazo)
              .HasForeignKey(c => c.IdMazo)
              .OnDelete(DeleteBehavior.Restrict);
        }

        public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
        {
            public ApplicationDbContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory()) // base path para buscar appsettings.json
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                optionsBuilder.UseSqlServer(connectionString);

                return new ApplicationDbContext(optionsBuilder.Options, configuration);
            }
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
        /// Ejecuta una consulta SQL utilizando Dapper y devuelve un solo resultado o el valor predeterminado si no hay resultados.
        /// </summary>
        /// <typeparam name="T">Tipo de los datos de retorno.</typeparam>
        /// <param name="text">Consulta SQL a ejecutar.</param>
        /// <param name="parameters">Parámetros opcionales de la consulta.</param>
        /// <param name="timeout">Tiempo de espera opcional para la consulta.</param>
        /// <param name="type">Tipo opcional de comando SQL.</param>
        /// <returns>Un objeto del tipo especificado o su valor predeterminado.</returns>
        public async Task<T?> QueryFirstOrDefaultAsync<T>(string text, object? parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, text, parameters ?? new { }, timeout, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(command.Definition);
        }

        /// <summary>
        /// Obtiene un IQueryable para usar en consultas LINQ que incluye filtro de status activo.
        /// </summary>
        /// <typeparam name="T">Tipo de entidad para la consulta.</typeparam>
        /// <returns>IQueryable filtrado para estrategias LINQ.</returns>
        public IQueryable<T> GetActiveSet<T>() where T : class
        {
            var query = Set<T>().AsQueryable();

            // Filtramos por Status aplicando expresiones genéricas si la entidad tiene la propiedad Status
            var parameter = Expression.Parameter(typeof(T), "e");

            if (typeof(T).GetProperty("Status") != null)
            {
                try
                {
                    // Construimos una expresión lambda para filtrar por Status = true
                    var property = Expression.Property(parameter, "Status");
                    var value = Expression.Constant(true);
                    var equal = Expression.Equal(property, value);
                    var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

                    // Aplicamos el filtro
                    query = query.Where(lambda);
                }
                catch
                {
                    // Si hay algún error, devolvemos el query sin filtrar
                }
            }

            return query;
        }

        /// <summary>
        /// Método auxiliar para obtener el valor de una propiedad de un objeto mediante reflexión.
        /// </summary>
        /// <param name="obj">Objeto del que se obtendrá el valor.</param>
        /// <param name="propertyName">Nombre de la propiedad.</param>
        /// <returns>Valor de la propiedad.</returns>
        private static bool GetPropertyValue(object obj, string propertyName)
        {
            var property = obj.GetType().GetProperty(propertyName);
            if (property == null)
            {
                return false;
            }
            return property.GetValue(obj, null) is bool value ? value : false;
        }

        /// <summary>
        /// Ejecuta una consulta LINQ y devuelve los resultados como una colección asíncrona.
        /// </summary>
        /// <typeparam name="T">Tipo de los datos de retorno.</typeparam>
        /// <param name="query">Consulta IQueryable a ejecutar.</param>
        /// <returns>Colección asíncrona de resultados.</returns>
        public async Task<List<T>> ToListAsyncSafe<T>(IQueryable<T> query)
        {
            if (query == null)
                return new List<T>();

            return await EntityFrameworkQueryableExtensions.ToListAsync(query);
        }


        /// <summary>
        /// Ejecuta una consulta con paginación utilizando LINQ.
        /// </summary>
        /// <typeparam name="T">Tipo de los datos de retorno.</typeparam>
        /// <param name="query">Consulta IQueryable base.</param>
        /// <param name="page">Número de página (comienza en 1).</param>
        /// <param name="pageSize">Tamaño de la página.</param>
        /// <returns>Colección paginada de elementos.</returns>
        public IQueryable<T> GetPaged<T>(IQueryable<T> query, int page, int pageSize) where T : class
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            return query.Skip((page - 1) * pageSize).Take(pageSize);
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