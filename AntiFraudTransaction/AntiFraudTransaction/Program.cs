using AntiFraudTransaction.Mapper;
using Confluent.Kafka;
using DB;
using Infrastructure.Gateway;
using Interfaces.Infrastucture;
using Interfaces.Manager;
using Interfaces.Repositories;
using Manager;
using Manager.Consumers;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration["ConnectionString"])
);

builder.Services.AddScoped<IEventConsumer, EventConsumer>();
builder.Services.Configure<ConsumerConfig>(
  builder.Configuration.GetSection(nameof(ConsumerConfig))
);

builder.Services.AddHttpClient<IAntiFraudGateway>();
builder.Services.AddScoped<ITransactionalOperationRepository, TransactionalOperationRepository>();
builder.Services.AddScoped<ITransactionalOperationManager, TransactionalOperationManager>();
builder.Services.AddScoped<IAntiFraudGateway, AntiFraudGateway>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
