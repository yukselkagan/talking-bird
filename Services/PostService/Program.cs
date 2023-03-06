using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Timeout;
using PostService.Clients;
using PostService.Data;
using PostService.Data.Repository;
using MassTransit;
using PostService.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

ServiceSettings serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumers(Assembly.GetEntryAssembly());
    cfg.UsingRabbitMq((context, configurator) =>
    {
        var rabbitMQSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        configurator.Host(rabbitMQSettings.Host);
        configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
    });
});

Random rand = new Random();
builder.Services.AddHttpClient<IdentityServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:44398");
})
//.AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
//    10,
//    TimeSpan.FromSeconds(5),
//    onBreak: (outcome, timepsan) =>
//    {
//        Console.WriteLine($"Opening circuit for {timepsan} seconds");
//    },
//    onReset: () =>
//    {
//        Console.WriteLine("Opening circuit");
//    }
//))
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(2));





builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

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
