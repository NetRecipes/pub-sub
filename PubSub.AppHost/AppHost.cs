using CommunityToolkit.Aspire.Hosting.Dapr;

var builder = DistributedApplication.CreateBuilder(args);

var redisPassword = builder.AddParameter("RedisPassword", true);
var redisPort = builder.AddParameter("RedisPort");
var redisPortValue = await redisPort.Resource.GetValueAsync(CancellationToken.None);

var redis = builder
    .AddRedis(
        "redis",
        int.Parse(redisPortValue!),
        redisPassword)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithRedisInsight();

var rabbitmqUsername = builder.AddParameter("RabbitMQUsername");
var rabbitmqPassword = builder.AddParameter("RabbitMQPassword", true);
var rabbitMQPort = builder.AddParameter("RabbitMQPort");
var rabbitMQPortValue = await rabbitMQPort.Resource.GetValueAsync(CancellationToken.None);

var rabbitmq = builder
    .AddRabbitMQ(
        "rabbitmq",
        rabbitmqUsername,
        rabbitmqPassword,
        int.Parse(rabbitMQPortValue!))
    .WithLifetime(ContainerLifetime.Persistent)
    .WithManagementPlugin();

var servicea = builder
    .AddProject<Projects.ServiceA>("servicea")
    .WaitFor(redis).WaitFor(rabbitmq)
    .WithDaprSidecar(new DaprSidecarOptions
    {
        ResourcesPaths = [Path.Combine("..", "components")]
    });

var serviceb = builder
    .AddProject<Projects.ServiceB>("serviceb")
    .WaitFor(redis).WaitFor(rabbitmq)
    .WithDaprSidecar(new DaprSidecarOptions
    {
        ResourcesPaths = [Path.Combine("..", "components")]
    });

builder.Build().Run();
