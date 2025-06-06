using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RuWitter1.Server.Models;
using Swashbuckle.AspNetCore.Filters;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddScoped<RuWitter1.Server.Interfaces.IMediaExtensionInterface, RuWitter1.Server.Services.MediaExtensionService>();
builder.Services.AddScoped<RuWitter1.Server.Interfaces.IMediaFileInterface, RuWitter1.Server.Services.MediaFileService>();
builder.Services.AddScoped<RuWitter1.Server.Interfaces.IDefaultUserInterface, RuWitter1.Server.Services.DefaultUserService>();
builder.Services.AddScoped<RuWitter1.Server.Interfaces.IPostInterface, RuWitter1.Server.Services.PostService>();
builder.Services.AddScoped<RuWitter1.Server.Interfaces.ICommentInterface, RuWitter1.Server.Services.CommentService>();
builder.Services.AddScoped<RuWitter1.Server.Interfaces.IChatInterface, RuWitter1.Server.Services.ChatService>();
builder.Services.AddScoped<RuWitter1.Server.Interfaces.IMessageInterface, RuWitter1.Server.Services.MessageService>();

builder.Services.AddDbContextPool<PostContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostContextPostgreSQL")));

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<DefaultUser>()
    .AddEntityFrameworkStores<PostContext>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<DefaultUser>();


app.MapPost("/logout", async (SignInManager<DefaultUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
}).RequireAuthorization();

app.MapGet("/pingauth", (ClaimsPrincipal user) =>
{
    var email = user.FindFirstValue(ClaimTypes.Email); // берёт email пользователя для claim
    return Results.Json(new {Email = email}); // возвращает email как обычный текст
}).RequireAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
