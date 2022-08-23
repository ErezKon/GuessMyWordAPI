using GuessMyWordAPI.DataLayer;
using GuessMyWordAPI.IServices;
using GuessMyWordAPI.Services;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpLoggerActionFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IWordService, WordService>();
builder.Services.AddSingleton<IMyLogger, MyLogger>();

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

var corsPolicy = "AllPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy,
        policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

builder.Services.AddDbContext<WordContext>(options =>
{

});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse("192.168.1.18"));
    options.KnownProxies.Add(IPAddress.Parse("213.57.171.199"));
});

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
        options.HttpsPort = 443;
    });
} 
else
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
        options.HttpsPort = 5001;
    });
}

builder.WebHost.UseKestrel(kestrel =>
{
    kestrel.Listen(IPAddress.Any, 5123);
    kestrel.Listen(IPAddress.Any, 7123);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

app.UseAuthorization();

app.MapControllers();

app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseHsts();

app.UseCors(corsPolicy);

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.Run();
