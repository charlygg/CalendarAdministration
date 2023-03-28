using Microsoft.Graph;

namespace CalendarAdministration.Services
{
    public interface IProfileService
    {
        public Task<User?> GetProfileAsync();
    }
}
