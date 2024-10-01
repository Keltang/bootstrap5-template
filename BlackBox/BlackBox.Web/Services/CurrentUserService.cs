using BlackBox.Application.Common.Interfaces;
using System.Security.Claims;

namespace BlackBox.Web.Services 
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string? _userId;
        public List<string>? _listUserRoles;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_userId))
                    _userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                return _userId;
            }
        }

        public List<string> UserRoles
        {
            get
            {
                if (_listUserRoles == null || !_listUserRoles.Any())
                    _listUserRoles = _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role)?.Select(x => x.Value).ToList();

                return _listUserRoles ?? new();
            }
        }

        public void SetUserId(string id)
        {
            _userId = id;
        }

    }
}
