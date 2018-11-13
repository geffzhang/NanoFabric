using NanoFabric.Mediatr.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleService.Kestrel.Application.CommandSide.Commands
{
    /// <summary>
    /// Echo Command
    /// </summary>
    public class EchoCommand : ICommand<string>
    {
        /// <summary>
        /// 输入
        /// </summary>
        public string Input { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="input"></param>
        public EchoCommand(
            string input
            )
        {
            Input = input;
        }
    }
}
