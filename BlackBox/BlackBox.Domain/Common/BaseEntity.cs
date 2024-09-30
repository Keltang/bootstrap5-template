 
namespace BlackBox.Domain.Common
{
    public abstract class BaseEntity<T>  
    {
        public T Id { get; set; }

        public bool IsDeleted { get; set; }

        public void GenerateId()
        {
            Id = typeof(T) == typeof(Guid) ? (T)(object)Guid.NewGuid() : default;
        }
    }
}
