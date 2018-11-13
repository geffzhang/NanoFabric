using System.Diagnostics;
using NanoFabric.Mediatr.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace NanoFabric.Mediatr.Exceptions
{
    [DebuggerStepThrough]
    public static class ThrowMediatrPipelineException
    {
        public static void IdentifiedCommandWithoutId()
        {
            throw new MediatrPipelineException(
                "Invalid IdentifiedCommand",
                new ValidationException("Validation Exception",
                    new[] { new ValidationFailure("Id", "You need to specify a valid id.") }
                )
            );
        }

        public static void IdentifiedCommandWithoutInnerCommand()
        {
            throw new MediatrPipelineException(
                "Invalid IdentifiedCommand",
                new ValidationException(
                    "Validation Exception",
                    new[] { new ValidationFailure("Command", "You need to specify a valid command to run.") }
                )
            );
        }

        public static void CommandWasAlreadyExecuted()
        {
            throw new MediatrPipelineException(
                "Invalid IdentifiedCommand",
                new ValidationException("Validation Exception",
                    new[]
                    {
                        new ValidationFailure("Id", "This command was already executed.")
                    }));
        }
    }
}