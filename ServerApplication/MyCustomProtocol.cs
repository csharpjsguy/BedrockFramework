﻿using System.Threading.Tasks;
using Bedrock.Framework.Protocols;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using Protocols;

namespace ServerApplication
{
    public class MyCustomProtocol : ConnectionHandler
    {
        private readonly ILogger _logger;

        public MyCustomProtocol(ILogger<MyCustomProtocol> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync(ConnectionContext connection)
        {
            // Use a length prefixed protocol
            var protocol = new LengthPrefixedProtocol();
            var reader = Protocol.CreateReader(connection, protocol);
            var writer = Protocol.CreateWriter(connection, protocol);

            while (true)
            {
                var message = await reader.ReadAsync();

                _logger.LogInformation("Received a message of {Length} bytes", message.Payload.Length);

                // REVIEW: We need a ReadResult<T> to indicate completion and cancellation
                if (message.Payload == null)
                {
                    break;
                }
            }
        }
    }
}