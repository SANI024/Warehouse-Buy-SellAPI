using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Warehouse_Buy_Sell.Data;
using Warehouse_Buy_Sell.Interfaces;
using Warehouse_Buy_Sell.Services;

var builder = WebApplication.CreateBuilder(args);

// DATABASE
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins("http://localhost:5127") // Swagger UI origin
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// SERVICES
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IInternalMovementService, InternalMovementService>();
builder.Services.AddScoped<IInverntoryService, InventoryService>();
builder.Services.AddScoped<IReportService, ReportService>();

// JWT AUTHENTICATION
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"JWT ERROR: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("JWT SUCCESS: Token validated!");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Warehouse & Purchase & Sell API",
        Version = "v1",
        Description = "Simple Purchase & Sale system"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});

var app = builder.Build();

// MIDDLEWARE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Purchase & Sale API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");       // ✅ CORS first
app.UseAuthentication();       // ✅ then auth
app.UseAuthorization();        // ✅ then authorization

// Optional: log headers to debug
app.Use(async (context, next) =>
{
    Console.WriteLine("HEADERS RECEIVED:");
    foreach (var h in context.Request.Headers)
        Console.WriteLine($"{h.Key}: {h.Value}");
    await next();
});

app.MapControllers();
app.Run();
