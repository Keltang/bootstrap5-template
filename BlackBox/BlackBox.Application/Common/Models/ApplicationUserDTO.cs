
namespace BlackBox.Application.Common.Models
{
    public class ApplicationUserDTO
    {
        public required string UserId { get; set; }
        public string EUID { get; set; } = string.Empty;
        public required string Email { get; set; } 
    }
}
