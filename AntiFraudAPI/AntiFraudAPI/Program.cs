using AntiFraudAPI.Mapper;
using AntiFraudAPI.Middleware;
using Domain;
using Interfaces.Managers;
using Interfaces.Repositories;
using Interfaces.Validators;
using Manager;
using Manager.Producer;
using Manager.Validators;
using Microsoft.Data.SqlClient;
using Repository;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration["ConnectionString"])
);

builder.Services.AddScoped<IOperationRepository, OperationRepository>();
builder.Services.AddScoped<IOperationManager, OperationManager>();
builder.Services.AddScoped<ICustomValidator<Operation>, OperationValidator>();
builder.Services.AddSingleton<IProducer, Producer>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<KafkaSettings>
       (builder.Configuration.GetSection(nameof(KafkaSettings)));

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

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
