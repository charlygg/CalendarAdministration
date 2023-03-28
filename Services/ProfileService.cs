using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace CalendarAdministration.Services
{
    public class ProfileService : IProfileService
    {
        private readonly GraphServiceClient _client;
        private readonly ILogger<ProfileService> _logger;
        
        public ProfileService(GraphServiceClient client,
            ILogger<ProfileService> logger)
        {
            _client = client;
            _logger = logger;
        }

        [AuthorizeForScopes(Scopes = new string[] { "user.read", "Mail.Read" })]
        public async Task<User?> GetProfileAsync()
        {
            var result = await _client.Me.Request().GetAsync();
            
            if (result == null) return null;

            return result;
        }
    }
}
