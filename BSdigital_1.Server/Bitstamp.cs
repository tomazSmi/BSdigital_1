using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace BSdigital_1.Server
{


    public class BitstampOrderBook
    {
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; } = "";

        [JsonPropertyName("microtimestamp")]
        public string Microtimestamp { get; set; } = "";

        [JsonPropertyName("bids")]
        public List<OrderBookEntry> Bids { get; set; } = [];

        [JsonPropertyName("asks")]
        public List<OrderBookEntry> Asks { get; set; } = [];
    }

    public class OrderBookEntry
    {
        [JsonPropertyName("price")]
        public string Price { get; set; } = "";

        [JsonPropertyName("amount")]
        public string Amount { get; set; } = "";

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }

    public class BitstampOrderBookRaw
    {
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; } = "";

        [JsonPropertyName("microtimestamp")]
        public string Microtimestamp { get; set; } = "";

        [JsonPropertyName("bids")]
        public List<string[]> Bids { get; set; } = new();

        [JsonPropertyName("asks")]
        public List<string[]> Asks { get; set; } = new();
    }

    public class BitstampClient
    {
        private readonly HttpClient _httpClient;

        public BitstampClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BitstampOrderBook> GetBtcEurOrderBookAsync(
            CancellationToken cancellationToken = default)
        {
            const string url =
                "https://www.bitstamp.net/api/v2/order_book/btceur/?group=1";

            using var response = await _httpClient.GetAsync(
                url,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var raw = await response.Content.ReadFromJsonAsync<BitstampOrderBookRaw>(
                cancellationToken);

            if (raw == null)
            {
                throw new InvalidOperationException(
                    "Bitstamp returned an empty order book.");
            }

            var orderBook = new BitstampOrderBook
            {
                Timestamp = raw.Timestamp,
                Microtimestamp = raw.Microtimestamp,
                Bids = raw.Bids?.Select(a => new OrderBookEntry
                {
                    Price = a.Length > 0 && a[0] != null ? a[0] : "",
                    Amount = a.Length > 1 && a[1] != null ? a[1] : ""
                }).ToList() ?? new List<OrderBookEntry>(),
                Asks = raw.Asks?.Select(a => new OrderBookEntry
                {
                    Price = a.Length > 0 && a[0] != null ? a[0] : "",
                    Amount = a.Length > 1 && a[1] != null ? a[1] : ""
                }).ToList() ?? new List<OrderBookEntry>()
            };

            return orderBook;
        }
    }
}
