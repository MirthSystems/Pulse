namespace Pulse.Clients.Web
{
    using Pulse.Core.Models.Requests;
    using Pulse.Core.Models.Responses;
    using System.Net.Http.Json;

    public class VenuesApiClient
    {
        private readonly HttpClient _httpClient;

        public VenuesApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<VenueListResponse> GetVenuesAsync(VenueQueryRequest request)
        {
            var queryString = $"api/venues?Page={request.Page}&PageSize={request.PageSize}";

            if (!string.IsNullOrEmpty(request.SearchTerm))
                queryString += $"&SearchTerm={Uri.EscapeDataString(request.SearchTerm)}";

            if (request.VenueTypeId.HasValue)
                queryString += $"&VenueTypeId={request.VenueTypeId.Value}";

            var response = await _httpClient.GetFromJsonAsync<VenueListResponse>(queryString);
            return response ?? new VenueListResponse();
        }

        public async Task<VenueDetailResponse?> GetVenueDetailAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<VenueDetailResponse>($"api/venues/{id}");
        }

        public async Task<VenueListResponse> GetManagedVenuesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<VenueListResponse>("api/venues/managed");
            return response ?? new VenueListResponse();
        }

        public async Task<VenueTypeListResponse> GetVenueTypesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<VenueTypeListResponse>("api/venues/types");
            return response ?? new VenueTypeListResponse();
        }

        public async Task<NewVenueResponse> CreateVenueAsync(NewVenueRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/venues", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<NewVenueResponse>() ??
                   throw new InvalidOperationException("Failed to deserialize response");
        }

        public async Task<UpdateVenueResponse> UpdateVenueAsync(int id, UpdateVenueRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/venues/{id}", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UpdateVenueResponse>() ??
                   throw new InvalidOperationException("Failed to deserialize response");
        }
    }
}
