using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SampleService.Kestrel.Application.CommandSide.Commands
{
    /// <summary>
    /// EchoCommand 处理器
    /// </summary>
    public class EchoCommandHandler
        : IRequestHandler<EchoCommand, string>
    {
        public Task<string> Handle(
            EchoCommand request,
            CancellationToken cancellationToken
            )
        {
            return Task.FromResult(request.Input);
        }
    }
}
