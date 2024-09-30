namespace BlackBox.Application.Common.Models
{
    public class IdNameDto<T>
    {
        public T? Id { get; set; }
        public string Name { get; set; } = string.Empty;    
    }
}
