using ASP_SPD111.Data;
using ASP_SPD111.MiddleWare;
using ASP_SPD111.Services.Hash;
using ASP_SPD111.Services.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// додаємо до конфігурації файл з даними підключення
builder.Configuration.AddJsonFile("dbsettings.json");

// Add services to the container.
builder.Services.AddControllersWithViews();

// реєструємо власні сервіси
builder.Services.AddSingleton<IHashService, Sha1HashService>();
builder.Services.AddSingleton<IValidationService, MyValidationService>();

// та контекст даних
String? connectionString = builder.Configuration.GetConnectionString("PlanetScale");
MySqlConnection connection = new(connectionString);

builder.Services.AddDbContext<DataContext>(opttions =>
    opttions.UseMySql(
        connection, 
        ServerVersion.AutoDetect(connection),
        serverOptions => 
            serverOptions
            .MigrationsHistoryTable(
                tableName: HistoryRepository.DefaultTableName,
                schema: "ASP_SPD111")
            .SchemaBehavior(
                MySqlSchemaBehavior.Translate,
                (schema, table) => $"{schema}_{table}")
    )
);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

// включення нашого MiddleWare у ланцюг
app.UseMiddleware<AuthSessionMiddleWare>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
