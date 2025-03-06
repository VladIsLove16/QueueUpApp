using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Регистрируем HttpClient
builder.Services.AddHttpClient();

// Добавляем сервисы
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gateway", Version = "v1" });
});

var app = builder.Build();

// **ВАЖНО:** Убедитесь, что этот код выполняется
app.UseRouting();
app.UseAuthorization();

// **Swagger должен быть вызван до MapControllers()**
app.UseSwagger();
app.UseSwaggerUI();


// **ВАЖНО:** Регистрация контроллеров
app.MapControllers();

app.Run();
