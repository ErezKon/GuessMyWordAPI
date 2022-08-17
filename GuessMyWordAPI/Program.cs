using GuessMyWordAPI.DataLayer;
using GuessMyWordAPI.IServices;
using GuessMyWordAPI.Services;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IWordService, WordService>();

builder.Services.AddDbContext<WordContext>(options =>
{

});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse("192.168.1.18"));
    options.KnownProxies.Add(IPAddress.Parse("213.57.171.199"));
});

builder.WebHost.UseKestrel(kestrel =>
{
    kestrel.Listen(IPAddress.Any, 5123);
    kestrel.Listen(IPAddress.Any, 7123);
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.Run();
