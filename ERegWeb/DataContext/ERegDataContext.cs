using ERegWeb.Domain;
using Microsoft.EntityFrameworkCore;

namespace ERegServer.DataContext
{
    /// <summary>
    /// Represents the Entity Framework Core data context for email-related data.
    /// </summary>
    public class ERegDataContext : DbContext
    {
        /// <summary>
        /// Gets or sets the DbSet for storing generated email codes.
        /// </summary>
        public DbSet<EmailCodeGenerated> EmailCodesGenerated { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ERegDataContext"/> class.
        /// </summary>
        /// <param name="options">The DbContext options.</param>
        public ERegDataContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Configures the database schema for email-related entities.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailCodeGenerated>().ToTable("EmailCodesGenerated");
        }
    }
}
