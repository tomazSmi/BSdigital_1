
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BSdigital_1.Server;

public class OrderBook
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("bids")]
    public List<OrderBookEntry> Bids { get; set; } = [];

    [JsonPropertyName("asks")]
    public List<OrderBookEntry> Asks { get; set; } = [];
}

public class OrderBookEntry
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [JsonPropertyName("price")]
    public string Price { get; set; } = "";

    [JsonPropertyName("amount")]
    public string Amount { get; set; } = "";

}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<OrderBook> OrderBooks => Set<OrderBook>();

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlite("Data Source=mydatabase.db");
    //}
}