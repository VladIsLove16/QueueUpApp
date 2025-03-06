using EmailServiceAPI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Подключаем конфигурацию
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Регистрируем EmailService
builder.Services.AddTransient<EmailService>();

// Добавляем поддержку контроллеров
builder.Services.AddControllers();

// Подключение Swagger
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Meeting API", Version = "v1" });
//});

var app = builder.Build();

//// Подключение Swagger UI
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
// Настройка маршрутов
app.MapControllers();

// Запуск приложения
app.Run();