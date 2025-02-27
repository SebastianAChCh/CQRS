using Autofac.Core;
using CQRS.Infrastructure;
using CQRS.Infrastructure.DataBase;
using CQRS.Infrastructure.DataBase.RepositoryPosts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(Program).Assembly);

builder.Services.AddDbContext<DataBaseContext>(op => {
    string connection = builder.Configuration.GetConnectionString("PostgresConnection");
    op.UseNpgsql(connection);//PostgresConnection is not created yet
});

builder.Services.AddStackExchangeRedisCache(op => {
    string connection = builder.Configuration.GetConnectionString("RedisConnection");
    op.Configuration = connection;
    op.InstanceName = "CacheApplication";   
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection"))
);

builder.Services.AddScoped<IPostRepository, PostsRepo>();
builder.Services.AddScoped<ICacheRepo, CacheRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
