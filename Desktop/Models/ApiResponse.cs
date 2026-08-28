using System.Text.Json.Serialization;

namespace FrutNatura.Desktop.Models
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("items")]
        public T Items { get; set; } = default!;

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
    }
}
