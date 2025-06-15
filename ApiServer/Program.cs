using ApiServer.RequestProcessors.Forecast;
using ApiServer.RequestProcessors.Models;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IForecastRequestProcessor, ForecastRequestProcessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapPost("/weatherforecast", (
    IForecastRequestProcessor forecastRequestProcessor,
    WeatherForecastRequest request) =>
{
    var forecast = forecastRequestProcessor.GetForecast(request);
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();


