using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka.Receive
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Começou a processar em: {DateTimeOffset.Now}");

            ConsumerConfig consumerConfig = new ConsumerConfig
            {
                GroupId = "email-consumer-group",
                BootstrapServers = "kafka:9093",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
            {
                consumerBuilder.Subscribe("fila_pedido");

                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumeResult = consumerBuilder.Consume(stoppingToken);
                        _logger.LogInformation($"Mensagem: {consumeResult.Message.Value} recebida de {consumeResult.TopicPartitionOffset}");

                        await Task.Delay(1000, stoppingToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    consumerBuilder.Close();
                }
            }
        }
    }
}
