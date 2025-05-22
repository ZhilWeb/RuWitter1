using Microsoft.EntityFrameworkCore;
using RuWitter1.Server.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<RuWitter1.Server.Interfaces.IMediaExtensionInterface, RuWitter1.Server.Services.MediaExtensionService>();
builder.Services.AddScoped<RuWitter1.Server.Interfaces.IMediaFileInterface, RuWitter1.Server.Services.MediaFileService>();

builder.Services.AddDbContextPool<PostContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostContextPostgreSQL")));


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
