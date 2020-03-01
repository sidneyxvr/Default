using Microsoft.EntityFrameworkCore;
using Default.Business.Models;
using System.Linq;

namespace Default.Data.Context
{
    /// <summary>
    /// Database configiration
    /// </summary>
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> optios) : base(optios) { }

        /// <summary>
        /// Configure database behavior
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DefaultContext).Assembly);

            #region default behavior
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                        .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            #endregion

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }
    }
}
