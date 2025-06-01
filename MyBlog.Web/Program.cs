using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyBlog.Web.Services;
using MyBlog.Persistance.Identity;
using MyBlog.Persistance.Database;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MyBlog.Persistance.Services;
using MyBlog.Persistance.Repositories.CommentRepository;
using MyBlog.Persistance.Repositories.PostRepository;
using MyBlog.Persistance.Repositories.ImageRepository;
using MyBlog.Persistance.Repositories.UserRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using MyBlog.Persistance.ExternalFeatures.SummarizationModelApis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(connectionString))
    .AddIdentity<ApplicationUser, IdentityRole>(config =>
    {
        config.Password.RequireNonAlphanumeric = false;
        config.Password.RequireUppercase = false;
        config.Password.RequiredLength = 6;
        config.User.RequireUniqueEmail = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IDbInitializer, DbInitializer>(); //My Seed service

builder.Services.AddScoped<IUserManager, UserManager>(); //My UserManager
builder.Services.AddScoped<IPostManager, PostManager>(); //My PostManager
builder.Services.AddScoped<ICommentManager, CommentManager>(); //My CommentManager
builder.Services.AddScoped<IImageManager, ImageManager>(); //My ImageManager
builder.Services.AddScoped<SummarizationModelHttpClient>(); //My Service for NLP model api  

//Cookie options for Identity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Accounts/Login";
    options.AccessDeniedPath = "/Accounts/Login";
});

builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"]!,
            ValidAudience = builder.Configuration["Jwt:Audience"]!,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "Admin");
    });

    options.AddPolicy("UserPolicy", policy =>
    {
        policy.RequireAssertion(x =>
        x.User.HasClaim(ClaimTypes.Role, "User") ||
        x.User.HasClaim(ClaimTypes.Role, "Admin")
        );
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

//Seeding db
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();
    if (dbInitializer != null)
    {
        //dbInitializer.Initialize();
        dbInitializer.SeedData();
    }
}

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseBlazorFrameworkFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.MapFallbackToFile("index.html");

app.Run();


