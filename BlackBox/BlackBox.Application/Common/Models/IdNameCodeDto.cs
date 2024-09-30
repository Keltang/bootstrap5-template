namespace BlackBox.Application.Common.Models
{
    public class IdNameCodeDto<T> : IdNameDto<T>
    {
        public string Code { get; set; } = string.Empty;
    }
}
