
using System.Threading.Channels;

namespace ChannelsDemo;

public class Processor : BackgroundService
{
    private readonly Channel<ChannelRequest> _channel;

    public Processor(Channel<ChannelRequest> channel)
    {
        _channel = channel;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var request = await _channel.Reader.ReadAsync(stoppingToken);

            await Task.Delay(2000);
            Console.WriteLine(request.Message);
        }
    }
}

public record ChannelRequest(string Message);