using Microsoft.EntityFrameworkCore;
using Sessions_app.Models;
using Sessions_app.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Sessions_app.Services;
using Sessions_app.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Sessions_app.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// 1. Configuração essencial para HATEOAS
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IUrlHelper>(x =>
{
    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
    return actionContext == null
        ? null
        : new UrlHelper(actionContext);
});

builder.Logging.AddConsole();

// 2. Configuração do Rate Limit
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;
    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;
        limiterOptions.Window = TimeSpan.FromSeconds(10);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 2;
    });
});

// 3. Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistema de Gestão de Riscos e Rotas",
        Version = "v1",
        Description = "API para gerenciamento de usuários, rotas seguras e riscos"
    });
    c.EnableAnnotations();
    c.CustomSchemaIds(type => type.FullName);
});

// 4. Configuração de sessão
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.Name = "_ApplicationSession";
    options.Cookie.IsEssential = true;
});

// 5. Configuração do banco de dados Oracle
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection"));
});

// 6. Registro de repositórios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IRiscoRepository, RiscoRepository>();
builder.Services.AddScoped<IRotaSeguraRepository, RotaSeguraRepository>();

// 7. Registro de serviços
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<RiscoService>();
builder.Services.AddScoped<RotaSeguraService>();

// 8. Serviço de ML.NET (Singleton para melhor performance)
// Alterar de Singleton para Scoped
builder.Services.AddScoped<RiskPredictionService>();

// 9. Serviços de mensageria (RabbitMQ)
builder.Services.AddSingleton<RabbitMqService>();
builder.Services.AddHostedService<RabbitMqConsumerService>();
builder.Services.AddScoped<EmailService>();

// 10. Configuração de contexto HTTP para HATEOAS
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

var rabbitService = app.Services.GetService<RabbitMqService>();

// Configuração do pipeline de requisições
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema de Gestão v1");
        c.RoutePrefix = "swagger";
        c.DefaultModelsExpandDepth(-1); // Oculta schemas no Swagger UI
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Aplica o Rate Limiting
app.UseRateLimiter();

app.UseAuthorization();
app.UseSession();

// Ativa os controllers da API
app.MapControllers();

// Configura rotas MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();