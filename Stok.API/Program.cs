using MassTransit;
using MongoDB.Driver;
using Shared;
using Stok.API.Consumers;
using Stok.API.Models.Entities;
using Stok.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedEventConsumer>();


    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqps://cqzmvjvj:sWnh_3prvLwKjGYCdtpEjOJwAN07uMzm@vulture.rmq.cloudamqp.com/cqzmvjvj");




        cfg.ReceiveEndpoint(RabbitMqSettings.Stock_Order_Created_Event_Queue, e =>
        {
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });


    });
});


builder.Services.AddSingleton<MongoDbService>();


using var scope = builder.Services.BuildServiceProvider().CreateScope();

var mongoDbService = scope.ServiceProvider.GetService<MongoDbService>();
var collection = mongoDbService.GetCollection<Stock>();
var ss = await collection.FindAsync(filter: x => true);
if (!ss.Any())
{
    await collection.InsertOneAsync(new Stock { Id = Guid.NewGuid().ToString(), ProductId = Guid.NewGuid().ToString(), Count = 1000 });
    await collection.InsertOneAsync(new Stock { Id = Guid.NewGuid().ToString(), ProductId = Guid.NewGuid().ToString(), Count = 2000 });
    await collection.InsertOneAsync(new Stock { Id = Guid.NewGuid().ToString(), ProductId = Guid.NewGuid().ToString(), Count = 3000 });
    await collection.InsertOneAsync(new Stock { Id = Guid.NewGuid().ToString(), ProductId = Guid.NewGuid().ToString(), Count = 4000 });
    await collection.InsertOneAsync(new Stock { Id = Guid.NewGuid().ToString(), ProductId = Guid.NewGuid().ToString(), Count = 5000 });
}








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
