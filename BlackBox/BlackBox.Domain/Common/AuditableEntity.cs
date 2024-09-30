
namespace BlackBox.Domain.Common
{
    public abstract class AuditableEntity<T> : BaseEntity<T> where T : IComparable<T>
    {
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }
         
    }
}
