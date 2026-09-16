using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Middlewares;
using NZWalks.API.Repositories;
using NZWalks.API.Repository;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Use Logging
// Trace => Debug => Information => Warning => Error => Critical
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/NzWalks_Log.txt", rollingInterval: RollingInterval.Minute)
    .MinimumLevel.Information()
    .CreateLogger();

// Bỏ Logger Provider mặc định
builder.Logging.ClearProviders();

// Tạo Serilog Logger mới
builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Add swagger doc 
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NZ Walks API", Version = "v1" });

    // Add authorization to Swagger
    // Thông báo Swagger sử dụng JWT sử dụng một security scheme tên là Bearer. Token sẽ được gửi trong HTTP Header có tên Authorization
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    // Yêu cầu Swagger sử dụng security scheme
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
}
);

builder.Services.AddDbContext<NZWalksDbContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("NZWalksConnectionString")
    )
);

builder.Services.AddDbContext<NZWalksAuthDbContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("NZWalksAuthConnectionString")
    )
);

builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

builder.Services.AddAutoMapper(configuration => configuration.AddProfile<AutoMapperProfiles>());

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        options => options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Nguồn phát hành, vd: https://localhost:7121/
            ValidateAudience = true, // Nơi nhận token, vd: https://localhost:7121/
            ValidateLifetime = true, // Thời hạn token
            ValidateIssuerSigningKey = true, // Chữ ký
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Issuer phải bằng giá trị trong appsettings.json "https://localhost:7121/"
            ValidAudience = builder.Configuration["Jwt:Audience"], // Audience phải bằng giá trị trong appsettings.json "https://localhost:7121/"

            // Dùng secret key trong appsettings.json để kiểm tra signature
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]
                )
            )
        }
    );

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Hiển thị thông tin file
app.UseStaticFiles(new StaticFileOptions
{
   // Truy cập thư mục hiện tại của images trên máy server
   FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "images")),

    // https://localhost:portnumber/images
    // Url để lấy được file
    RequestPath = "/images"
});

app.MapControllers();

app.Run();
