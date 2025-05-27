using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Directly read from appsettings.json
var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
var username = builder.Configuration["ApiSettings:Username"];
var password = builder.Configuration["ApiSettings:Password"];
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient("iHC", client =>
{
    client.BaseAddress = new Uri("https://fa-euvo-test-saasfaprod1.fa.ocs.oraclecloud.com");
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json")); // Standard JSON
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/vnd.oracle.adf.resourcecollection+json"));

    // Basic Auth
    var creds = $"{username}:{password}";
    var base64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(creds));
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Basic", base64);

    // Common headers for Oracle Cloud
    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
