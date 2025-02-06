using ChannelsDemo;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<Processor>();



builder.Services.AddSingleton<Channel<ChannelRequest>>(
    _ => Channel.CreateUnbounded<ChannelRequest>(new UnboundedChannelOptions
    {
        SingleReader = true,
        AllowSynchronousContinuations = false,
    }));

var app = builder.Build();

app.MapGet("send", async (Channel<ChannelRequest> channel) =>
{
    await channel.Writer.WriteAsync(new ChannelRequest($"Hello from {DateTime.UtcNow}"));
    return Results.Ok();
})
.WithName("Send");

app.Run();