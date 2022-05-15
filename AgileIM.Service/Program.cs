using AgileIM.IM.Helper;
using AgileIM.Service.OAuth;
using AgileIM.Service.OAuth.Configs;
using AgileIM.Service.Services;
using AgileIM.Service.Services.FriendService;
using AgileIM.Service.Services.FriendService.Impl;
using AgileIM.Service.Services.Ide4Service;
using AgileIM.Service.Services.Ide4Service.Impl;
using AgileIM.Service.Services.ImService;
using AgileIM.Service.Services.ImService.Impl;
using AgileIM.Service.Services.UserService;
using AgileIM.Service.Services.UserService.Impl;
using AgileIM.Shared.EFCore;
using AgileIM.Shared.EFCore.Data.Repository;
using AgileIM.Shared.EFCore.Data.UnitOfWork;
using AgileIM.Shared.EFCore.DbContexts;
using AgileIM.Shared.Models.Friend.Entity;
using AgileIM.Shared.Models.Users.Entity;

using IdentityServer4.Validation;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IVerifyService, VerifyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFriendService, FriendService>();
builder.Services.AddScoped<IImService, ImService>();

builder.Services.RegisterUnitOfWork<AgileImDbContext>();
builder.Services.RegisterRepository<Friend, FriendRepository>();
builder.Services.RegisterRepository<User, UserRepository>();

var ide4Add = builder.Configuration["ServerIpPort"];
builder.Services
    .AddIdentityServer(option => option.IssuerUri = ide4Add)
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


builder.Services
       .AddAuthentication("Bearer")
       .AddIdentityServerAuthentication(options =>
       {
           options.Authority = ide4Add;
           options.ApiName = "ImService";
           options.RequireHttpsMetadata = false;
       });

builder.Services.AddDbContext<AgileImDbContext>(options =>
{
    var sqlServerConnStr = builder.Configuration["SqlServerConnStr"];
    var sqLiteConnStr = builder.Configuration["SqLiteConnStr"];
    //options.UseSqlServer(sqlServerConnStr);
    options.UseSqlite(sqLiteConnStr);
});



builder.WebHost.UseUrls(builder.Configuration["ServerIpPort"]);
var app = builder.Build();
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope())
{
    var context = serviceScope?.ServiceProvider.GetRequiredService<AgileImDbContext>();
    context?.Database.EnsureCreated();
}


// 鉴权与授权
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseImServer();

app.MapControllers();

IdentityModelEventSource.ShowPII = true;

app.UseIdentityServer();

app.Run();
