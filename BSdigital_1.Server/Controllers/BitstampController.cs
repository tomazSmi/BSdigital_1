using BSdigital_1.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BSdigital_1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BitstampController : ControllerBase
    {
        private readonly BitstampClient _bitstamp;

        public BitstampController(BitstampClient bitstamp)
        {
            _bitstamp = bitstamp;
        }

        [HttpGet("btceur/orderbook")]
        public async Task<IActionResult> GetOrderBook(
            CancellationToken cancellationToken)
        {
            var orderBook = await _bitstamp.GetBtcEurOrderBookAsync(
                cancellationToken);

            return Ok(orderBook);
        }
    }
}
