using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace BSdigital_1.Server
{

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

        public async Task<OrderBook> GetBtcEurOrderBookAsync(
            CancellationToken cancellationToken = default)
        {
            var limit = 20; // Limit the number of bids and asks to 20
            const string url = "https://www.bitstamp.net/api/v2/order_book/btceur/?group=1"; // Group 1 already summs orders by price

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

            var orderBook = new OrderBook
            {
                Timestamp = raw.Timestamp,
                //Microtimestamp = raw.Microtimestamp,
                Bids = raw.Bids?.Take(limit).Select(a => new OrderBookEntry
                {
                    Price = a.Length > 0 && a[0] != null ? a[0] : "",
                    Amount = a.Length > 1 && a[1] != null ? a[1] : ""
                }).ToList() ?? new List<OrderBookEntry>(),
                Asks = raw.Asks?.Take(limit).Reverse().Select(a => new OrderBookEntry
                {
                    Price = a.Length > 0 && a[0] != null ? a[0] : "",
                    Amount = a.Length > 1 && a[1] != null ? a[1] : ""
                }).ToList() ?? new List<OrderBookEntry>()
            };

            return orderBook;
        }
    }
}
