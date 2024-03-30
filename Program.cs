using demoAPI.BLL;
using demoAPI.BLL.Common;
using demoAPI.BLL.DS;
using demoAPI.BLL.DSItems;
using demoAPI.BLL.Kanbans;
using demoAPI.BLL.Member;
using demoAPI.BLL.Todolist;
using demoAPI.BLL.Trips;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Middleware;
using demoAPI.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
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
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

//Authentication
var key = "This is my test key";

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddSingleton<IJwtAuthenticationHelper>(new JwtAuthenticationHelper(key));
//END================

//dependency
builder.Services.AddScoped<IMemberBLL, MemberBLL>();
builder.Services.AddScoped<IDSItemBLL, DSItemBLL>();
builder.Services.AddScoped<IDSItemSubBLL, DSItemSubBLL>();
builder.Services.AddScoped<ITodolistBLL, TodolistBLL>();
builder.Services.AddScoped<ITodolistDoneBLL, TodolistDoneBLL>();
builder.Services.AddScoped<IDSBLL, DSBLL>();
builder.Services.AddScoped<ICommonBLL, CommonBLL>();
builder.Services.AddScoped<IDSAccountBLL, DSAccountBLL>();
builder.Services.AddScoped<ITripBLL, TripBLL>();
builder.Services.AddScoped<IKanbanBLL, KanbanBLL>();
//END================

builder.Services.AddDbContext<DSContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DSConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    GlobalVars.DSTransactions = new System.Collections.Concurrent.ConcurrentDictionary<int, List<demoAPI.Model.DS.DSTransactionDtoV2>>();

    var services = scope.ServiceProvider;
    try
    {
        var contextDS = services.GetRequiredService<DSContext>();
        DbDSInitializer.Initialize(contextDS);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseMyMiddleware();
app.UseMiddleware<ExMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
