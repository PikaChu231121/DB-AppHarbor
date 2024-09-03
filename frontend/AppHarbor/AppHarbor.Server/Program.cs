using System.Text.Json.Serialization;
using AppHarbor.Server;
using AppHarbor.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// 配置数据库上下文  
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

// 配置控制器和 JSON 序列化选项  
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // 处理循环引用  
    });

builder.Services.AddEndpointsApiExplorer();

// 注释掉Swagger的相关服务注册（根据需求调整）  
builder.Services.AddSwaggerGen();

// 配置CORS策略  
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.SetIsOriginAllowed(origin => true)
                   .AllowAnyHeader()
                   .AllowCredentials()
                   .AllowAnyMethod();
        });
});

// 配置大文件上传限制  
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 1048576000; // 设置大小限制为1GB  
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1048576000; // 设置大小限制为1GB
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // 注释掉Swagger的中间件（根据需求调整）  
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 配置上传文件的静态文件中间件  
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

// 使用 MapFallbackToFile 来处理未匹配的路由
app.MapFallbackToFile("index.html");

app.Run();
