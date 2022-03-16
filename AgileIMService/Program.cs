using AgileIM.IM.Helper;
using AgileIM.Service.OAuth;
using AgileIM.Service.OAuth.Configs;
using AgileIM.Service.Service;

using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IVerifyService, VerifyService>();
builder.Services
    .AddIdentityServer()
    .AddDeveloperSigningCredential(true, "tempkey.jwk")
    // 客户端配置添加到内存中
    .AddInMemoryClients(Ide4Config.GetApiClients)
    .AddInMemoryApiScopes(Ide4Config.GetApiScopes)
    // 添加对OpenID Connect的支持
    .AddInMemoryIdentityResources(Ide4Config.GetIdentityResources)
    //把受保护的Api资源添加到内存中
    .AddInMemoryApiResources(Ide4Config.GetApiResource)
    // 用户验证
    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
    .AddProfileService<ProfileService>();

builder.WebHost.UseUrls("http://*:9659");
var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseImServer();

app.UseAuthorization().UseAuthentication();

app.MapControllers();

IdentityModelEventSource.ShowPII = true;

app.UseIdentityServer();

app.Run();
