using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;
using MundoFinanceiro.Database.Utils;
using MundoFinanceiro.Shared.Extensions;

namespace MundoFinanceiro.Database.Persistence
{
    internal class DataContext : DbContext
    {
        private readonly string _connectionString;
        
        public DataContext() : this(Settings.ConnectionString)
        {
#if !DEBUG
     throw new ApplicationException("Não é permitido instanciar o DataContext sem connection string em produção.");       
#endif
        }

        public DbSet<Fundamento> Fundamentos { get; set; }
        public DbSet<Papel> Papeis { get; set; }

        public DataContext(string connectionString) 
            => _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
            => optionsBuilder.UseNpgsql(_connectionString);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Lower case all columns and tables (configuration for PostgreSQL)
            foreach(var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.Relational().TableName = entity.Relational().TableName.ToSnakeCase();

                // Replace column names            
                foreach(var property in entity.GetProperties())
                    property.Relational().ColumnName = property.Relational().ColumnName.ToSnakeCase();

                foreach(var key in entity.GetKeys())
                    key.Relational().Name = key.Relational().Name.ToSnakeCase();

                foreach(var key in entity.GetForeignKeys())
                    key.Relational().Name = key.Relational().Name.ToSnakeCase();

                foreach(var index in entity.GetIndexes())
                    index.Relational().Name = index.Relational().Name.ToSnakeCase();
            }
        }
    }

}