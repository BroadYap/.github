using BroadYap.Hubs;
using BroadYap.DataService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<SharedDb>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("reactapp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseRouting();
app.UseCors("reactapp");

app.MapControllers();
app.MapHub<ChatHub>("/chat");

app.Run();
