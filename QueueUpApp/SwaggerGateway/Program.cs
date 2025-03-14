using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ������������ HttpClient
builder.Services.AddHttpClient();

// ��������� �������
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gateway", Version = "v1" });
});

var app = builder.Build();

// **�����:** ���������, ��� ���� ��� �����������
app.UseRouting();
app.UseAuthorization();

// **Swagger ������ ���� ������ �� MapControllers()**
app.UseSwagger();
app.UseSwaggerUI();


// **�����:** ����������� ������������
app.MapControllers();

app.Run();
