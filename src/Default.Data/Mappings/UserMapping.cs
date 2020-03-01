using Default.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Default.Data.Mappings
{
    /// <summary>
    /// Configure model user to database table
    /// </summary>
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.Property(a => a.Name).HasColumnType("varchar(100)");

            builder.Property(a => a.Email).HasColumnType("varchar(100)");
        }
    }
}
