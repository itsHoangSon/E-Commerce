using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.MassTransit
{
    public class RabbitMqOptions
    {
        public string Host { get; set; }
        public string ExchangeName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ushort Port { get; set; } = 5672;
        public string VirtualHost { get; set; }
        public bool UseMessageRetry { get; set; }
    }
}
