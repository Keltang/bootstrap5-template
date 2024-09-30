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

            builder.Property(p => p.ExternalId).IsRequired();
            builder.Property(p => p.Code).IsRequired().HasMaxLength(DataSizeConstants.ShortTitleTextLength);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(DataSizeConstants.TitleTextLength);
            builder.Property(p => p.OrderNumber).IsRequired();
            builder.Property(p => p.SubjectAreaId).IsRequired();

            builder.HasIndex(x => new { x.SubjectAreaId, x.Code }).IsUnique(); 
             
            builder.HasOne<ApplicationUser>().WithMany().HasForeignKey(new string[] { "CreatedBy" });
            //builder.HasOne<SubjectArea>().WithMany().HasForeignKey(new string[] { "SubjectAreaId" });
        }
    }
}
