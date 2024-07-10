using System.Text.Json.Serialization;
using AppHarbor.Server;
using AppHarbor.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // 添加 JSON 序列化配置以处理循环引用，能够同时查询两张表
    });

builder.Services.AddEndpointsApiExplorer();
// 注释掉Swagger的相关服务注册
builder.Services.AddSwaggerGen();

// 配置CORS策略
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // 注释掉Swagger的中间件
    app.UseSwagger();
    app.UseSwaggerUI();
}

// �ϴ��ļ�����
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/uploads"
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowSpecificOrigin"); // 启用CORS策略

app.UseAuthorization();

app.MapControllers();

app.Run();
