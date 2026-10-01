using BSdigital_1.Server;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


//string connectionString = "Data Source=mydatabase.db;";
//SqliteConnection connection = new SqliteConnection(connectionString);

//try
//{
//    connection.Open();
//    string createTableSql = "CREATE TABLE IF NOT EXISTS orderBooks (id INTEGER PRIMARY KEY AUTOINCREMENT, timestamp TEXT, name TEXT, json TEXT)";
//    SqliteCommand createTableCommand = new SqliteCommand(createTableSql, connection);
//    createTableCommand.ExecuteNonQuery();

//    Console.WriteLine("Connected to SQLite!");
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"Error: {ex.Message}");
//}
//finally
//{
//    connection.Close();
//}

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=mydatabase.db"));

builder.Services.AddHttpClient<BitstampClient>();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
