namespace BlackBox.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        List<string> UserRoles { get; }
        void SetUserId(string id);
    }
}
