using BSdigital_1.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BSdigital_1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BitstampController : ControllerBase
    {
        private readonly BitstampClient _bitstamp;
        private readonly AppDbContext _db;

        public BitstampController(BitstampClient bitstamp, AppDbContext db)
        {
            _bitstamp = bitstamp;
            _db = db;
        }

        [HttpGet("btceur/orderbook")]
        public async Task<IActionResult> GetOrderBook(CancellationToken cancellationToken)
        {
            var orderBook = await _bitstamp.GetBtcEurOrderBookAsync(cancellationToken);

            _db.OrderBooks.Add(new OrderBook
            {
                Timestamp = DateTime.UtcNow.ToString("o"),
                Name = "BtcEur",
                Bids = SumOrderBookEntries(orderBook.Bids),
                Asks = SumOrderBookEntries(orderBook.Asks),
            });

            await _db.SaveChangesAsync(cancellationToken);

            return Ok(orderBook);
        }


        [HttpGet("btceur/orderbook/log")]
        public async Task<IActionResult> GetOrderBookLog(CancellationToken cancellationToken)
        {
            var list = await _db.OrderBooks
                .Include("Asks")
                .Include("Bids")
                .Where(o => o.Name == "BtcEur")
                .OrderByDescending(o => o.Id)
                .Take(10)
                .ToListAsync(cancellationToken);

            return Ok(list);
        }

        // This is already done by the API if you use parameter group=1, but I will leave this here in case it is needed in the future.
        static List<OrderBookEntry> SumOrderBookEntries(List<OrderBookEntry> entries)
        {
            var sumDict = new Dictionary<string, decimal>();
            foreach (var item in entries)
            {
                if (!sumDict.ContainsKey(item.Price))
                {
                    sumDict.Add(item.Price, decimal.Parse(item.Amount));
                }
                else
                {
                    sumDict[item.Price] = sumDict[item.Price] + decimal.Parse(item.Amount);
                }
            }
            return sumDict.Select(i => new OrderBookEntry { Price = i.Key, Amount = i.Value.ToString() }).ToList();
        }
    }

}
