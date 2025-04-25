namespace Pulse.Clients.Web.Services
{
    public class PulseApiService
    {
        private readonly HttpClient _httpClient;

        public PulseApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}