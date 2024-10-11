using HousingLocation.Contexts;
using HousingLocation.Dto;
using HousingLocation.Models;
using HousingLocation.Service;
using HousingLocation.Service.IService;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthentication()
    .AddCookie(o =>
    {
        o.ExpireTimeSpan = TimeSpan.FromDays(2);

        o.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<HousingLocationContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"));
});

builder.Services.AddScoped<IHousingService, HousingService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserCardService, UserCardService>();

//builder.Services.AddAntiforgery();

builder.Services.AddAutoMapper(option =>
{
    option.CreateMap<House, HouseDto>().ReverseMap();
    option.CreateMap<Photo, PhotoDto>().ReverseMap();
    option.CreateMap<User, UserDto>().ReverseMap();
    option.CreateMap<UserCard, UserCardDto>().ReverseMap();
    
});
    
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opt =>
{
    //opt.AddPolicy("AllowAll", buil => buil.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

    opt.AddPolicy("OnlySite",
        buil => buil
            .WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});








var app = builder.Build();







app.UseCors("OnlySite");

//app.UseRouting();

//app.UseAntiforgery();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

record Credentials(string login, string password);
















/*
 
 
 var group = app.MapGroup("/api/auth");


var userManager = new Dictionary<string, string>()
{
    { "admin", "admin123" }
};


group.MapPost("/login", async ([FromBody] Credentials credentials, HttpContext context) =>
{
    if (!userManager.TryGetValue(credentials.login, out var pass))
    {
        return Results.BadRequest("Invalid login");
    }

    if (!string.Equals(credentials.password, pass, StringComparison.Ordinal))
    {
        return Results.BadRequest("Invalid password");
    }

    await context.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        new ClaimsPrincipal(
            new ClaimsIdentity(
                [new Claim("login", credentials.login), new Claim("userrole", "user")],
                CookieAuthenticationDefaults.AuthenticationScheme
            )
        ),
        new AuthenticationProperties { IsPersistent = true }
    );

    return Results.Ok();
});

group.MapGet("/logout", async ctx =>
{
    await ctx.SignOutAsync();
}).RequireAuthorization();

group.MapGet("/userprofile", (HttpContext ctx) =>
{
    return ctx.User.Claims.ToDictionary(x => x.Type, x => x.Value);
}).RequireAuthorization(pb =>
{
    pb.RequireClaim("login");
    pb.AuthenticationSchemes = new List<string>() { CookieAuthenticationDefaults.AuthenticationScheme };
});
 
 
 
 */
