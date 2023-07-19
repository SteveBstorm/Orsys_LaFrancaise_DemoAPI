using DemoWebAPI.Hubs;
using DemoWebAPI.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DataContext>();
builder.Services.AddSingleton<MyHub>();

builder.Services.AddSignalR();

builder.Services.AddCors(options => options.AddPolicy(
        "BlazorPolicy", o => o.WithOrigins("https://localhost:7225/")
                             .WithMethods("GET", "POST").AllowAnyHeader()
                             .AllowCredentials()
    )); ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseCors("BlazorPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MyHub>("articleHub");
app.MapHub<ChatHub>("chatHub");
app.Run();
