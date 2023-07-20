using DemoWebAPI.Hubs;
using DemoWebAPI.Infrastructure;
using DemoWebAPI.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DataContext>();
builder.Services.AddSingleton<MyHub>();
builder.Services.AddScoped<TokenManager>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("TokenInfo").GetSection("SecretKey").Value)),
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidAudience = "https://monAppclient.com",
            ValidateIssuer = true,
            ValidIssuer = "https://monserver.com"
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("IsConnected", policy => policy.RequireAuthenticatedUser());
});

builder.Services.AddSignalR();

builder.Services.AddCors(options => options.AddPolicy(
        "BlazorPolicy", o => o.WithOrigins("https://localhost:7225/")
                             .AllowAnyMethod().AllowAnyHeader()
                             .AllowCredentials()
    )); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
//app.UseCors("BlazorPolicy");
app.UseHttpsRedirection();

//Obligatoirement dans cet ordre
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<MyHub>("articleHub");
app.MapHub<ChatHub>("chatHub");
app.Run();
