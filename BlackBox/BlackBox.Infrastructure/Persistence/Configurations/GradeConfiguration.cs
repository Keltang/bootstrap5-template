using BlackBox.Domain.Entities;
using BlackBox.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BlackBox.Domain.Common;

namespace BlackBox.Infrastructure.Persistence.Configurations
{
    internal class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable($"{nameof(Grade)}s"); 

            builder.Property(p => p.Code).IsRequired().HasMaxLength(DataSizeConstants.ShortTitleTextLength);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(DataSizeConstants.TitleTextLength); 

            builder.HasIndex(x => new { x.Name, x.Code }).IsUnique(); 
             
            builder.HasOne<ApplicationUser>().WithMany().HasForeignKey(new string[] { "CreatedBy" }); 
        }
    }
}
