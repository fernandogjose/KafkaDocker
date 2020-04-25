﻿using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace Kafka.Sender.Controllers
{
    [Route("api/sender")]
    [ApiController]
    public class SenderController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody] Email request)
        {
            return Ok(SendMessageByKafka(request));
        }

        private string SendMessageByKafka(Email request)
        {
            var config = new ProducerConfig { BootstrapServers = "kafka:9093" };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var sendResult = producer.ProduceAsync("fila_pedido", new Message<Null, string> { Value = JsonConvert.SerializeObject(request) }).GetAwaiter().GetResult();
                    return $"Mensagem '{sendResult.Value}' de '{sendResult.TopicPartitionOffset}'";
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }

            return string.Empty;
        }
    }
}

public class Email
{
    public string Subject { get; set; }

    public string Body { get; set; }
}