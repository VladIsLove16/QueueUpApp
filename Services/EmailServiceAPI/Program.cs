using EmailServiceAPI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ���������� ������������
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// ������������ EmailService
builder.Services.AddTransient<EmailService>();

// ��������� ��������� ������������
builder.Services.AddControllers();

// ����������� Swagger
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Meeting API", Version = "v1" });
//});

var app = builder.Build();

//// ����������� Swagger UI
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
// ��������� ���������
app.MapControllers();

// ������ ����������
app.Run();