using API.Handlers;
using API.OperationFilters;
using API.Security.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });

    //new ApiScope("weatherApi.read", "Read Access to Weather API"),
    //            new ApiScope("weatherApi.write", "Write Access to Weather API"),
    //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    //{
    //    Type = SecuritySchemeType.OAuth2,
    //    Flows = new OpenApiOAuthFlows
    //    {
    //        ClientCredentials = new OpenApiOAuthFlow
    //        {
    //            TokenUrl = new Uri($"https://localhost:5018/connect/token"),
    //            Scopes = new Dictionary<string, string>
    //            {
    //                {"weatherApi.read", "Read Access to Weather API"}
    //            }
    //        },
    //        //AuthorizationCode = new OpenApiOAuthFlow
    //        //{
    //        //    TokenUrl = new Uri($"https://localhost:5018/connect/token"),
    //        //    Scopes = new Dictionary<string, string>
    //        //    {
    //        //        {"weatherApi.write", "Write Access to Weather API"}
    //        //    }
    //        //}
    //    }
    //});

    options.OperationFilter<AuthorizeOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5018";
        options.Audience = "weatherApi";
    });
    //.AddJwtBearer(options =>
    //{
    //    options.TokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateIssuer = true,
    //        ValidateAudience = true,
    //        ValidateLifetime = true,
    //        ValidateIssuerSigningKey = true,
    //        ValidIssuer = "Secret.123",
    //        ValidAudience = "Secret.123",
    //        IssuerSigningKey = JwtSecurityKey.Create("Secret.123")
    //    };
    //});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope1", builder =>
    {
        builder.RequireAuthenticatedUser();
        builder.RequireClaim("scope", "weatherApi.read");
    });

    options.AddPolicy("ApiScope2", builder =>
    {
        builder.RequireAuthenticatedUser();
        builder.RequireClaim("scope", "weatherApi.write");
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddTransient<ProtectedApiBearerTokenMiddleWare>();

var app = builder.Build();

// Configure the HTTP request pipeline.

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();
    return forecast;
});

app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
app.UseMiddleware<ProtectedApiBearerTokenMiddleWare>();
app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => { 
    endpoints.MapControllers(); 
});


app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}