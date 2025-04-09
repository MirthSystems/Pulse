namespace Pulse.Clients.Web
{
    using Pulse.Core.Models.Requests;
    using Pulse.Core.Models.Responses;
    using System.Net.Http.Json;

    public class SpecialsApiClient
    {
        private readonly HttpClient _httpClient;

        public SpecialsApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SpecialListResponse> GetSpecialsAsync(SpecialQueryRequest request)
        {
            var queryString = $"api/specials?Page={request.Page}&PageSize={request.PageSize}";

            if (!string.IsNullOrEmpty(request.SearchTerm))
                queryString += $"&SearchTerm={Uri.EscapeDataString(request.SearchTerm)}";

            if (request.VenueId.HasValue)
                queryString += $"&VenueId={request.VenueId.Value}";

            if (request.SpecialTypeId.HasValue)
                queryString += $"&SpecialTypeId={request.SpecialTypeId.Value}";

            if (request.TagId.HasValue)
                queryString += $"&TagId={request.TagId.Value}";

            var response = await _httpClient.GetFromJsonAsync<SpecialListResponse>(queryString);
            return response ?? new SpecialListResponse();
        }

        public async Task<SpecialDetailResponse?> GetSpecialDetailAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<SpecialDetailResponse>($"api/specials/{id}");
        }

        public async Task<SpecialTypeListResponse> GetSpecialTypesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<SpecialTypeListResponse>("api/specials/types");
            return response ?? new SpecialTypeListResponse();
        }

        public async Task<TagListResponse> GetTagsAsync(string? searchTerm = null)
        {
            var queryString = "api/specials/tags";

            if (!string.IsNullOrEmpty(searchTerm))
                queryString += $"?searchTerm={Uri.EscapeDataString(searchTerm)}";

            var response = await _httpClient.GetFromJsonAsync<TagListResponse>(queryString);
            return response ?? new TagListResponse();
        }

        public async Task<NewSpecialResponse> CreateSpecialAsync(NewSpecialRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/specials", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<NewSpecialResponse>() ??
                   throw new InvalidOperationException("Failed to deserialize response");
        }

        public async Task<UpdateSpecialResponse> UpdateSpecialAsync(int id, UpdateSpecialRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/specials/{id}", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UpdateSpecialResponse>() ??
                   throw new InvalidOperationException("Failed to deserialize response");
        }

        public async Task DeleteSpecialAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/specials/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
