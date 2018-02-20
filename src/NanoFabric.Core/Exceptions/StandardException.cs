using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NanoFabric.Core.Exceptions
{
    /// <summary>
    /// Represents an exception, which implements the <see cref="IStandardError"/> interface.
    /// </summary>
    public class StandardException : Exception, IStandardError
    {
        private string _message;
        private List<IStandardError> _details = new List<IStandardError>();

        /// <summary>
        /// Creates an <see cref="StandardException"/> indicating a failed invoke operation, with a <paramref name="message"/> if applicable.
        /// </summary>
        /// <param name="code">The code to represents the error.</param>
        /// <param name="message">An message which description caused the operation to fail.</param>
        /// <param name="target">The target of the particular error (e.g., the name of the property in error), the default value is the calloer member name</param>
        /// <returns>An <see cref="StandardException"/> indicating a failed invoke operation, with a <paramref name="message"/> if applicable.</returns>
        public static StandardException Caused(string code, string message, [CallerMemberName]string target = null) => new StandardException
        {
            Code = code,
            Message = message,
            Target = target,
        };

        /// <summary>
        /// Gets or sets the code for this error.
        /// </summary>
        /// <value>
        /// The code for this error.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the message for this error.
        /// </summary>
        /// <value>
        /// The message for this error.
        /// </value>
        public new string Message
        {
            get
            {
                if (string.IsNullOrEmpty(_message))
                {
                    return base.Message;
                }

                return _message;
            }
            set => _message = value;
        }

        /// <summary>
        /// Gets or sets the target of the particular error (e.g., the name of the property in error).
        /// </summary>
        /// <value>The target of the particular error.</value>
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the details for this error.
        /// </summary>
        /// <value>The details for this error.</value>
        public IEnumerable<IStandardError> Details
        {
            get => _details;
            set
            {
                if (value != null)
                {
                    _details.Clear();
                    _details.AddRange(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the inner error for this error.
        /// </summary>
        /// <value>The inner error for this error.</value>
        public IStandardError InnerError { get; set; }

        /// <summary>
        /// Throws this exception.
        /// </summary>
        public void Throw() => throw this;

        /// <summary>
        /// Appends an array of <see cref="IStandardError"/> represents the details to current <see cref="IStandardError"/>.
        /// </summary>
        /// <param name="details">The array of <see cref="IStandardError"/> represents the details.</param>
        /// <returns>Return the current <see cref="StandardException"/> instance for supporting fluent API feature.</returns>
        public StandardException Append(params IStandardError[] details)
        {
            if (details.Length > 0)
            {
                _details.AddRange(details);
            }

            // this for fluent API use
            // InvokeError.Caused(message).Append(details).Throw();
            return this;
        }

        /// <summary>
        /// Appends an <see cref="Exception"/> to current <see cref="IStandardError"/>.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> represents the details.</param>
        /// <returns>Return the current <see cref="StandardException"/> instance for supporting fluent API feature.</returns>
        public StandardException Concat(Exception exception)
        {
            var error = new StandardException
            {
                Code = "excption",
                Message = exception.ToString(),
            };

            _details.Add(error);

            // this for fluent API use
            // InvokeError.Caused(message).Append(details).Throw();
            return this;
        }

        /// <summary>
        /// Appends an array of <see cref="IStandardError"/> represents the details to current <see cref="IStandardError"/>.
        /// </summary>
        /// <param name="details">The array of <see cref="IStandardError"/> represents the details.</param>
        /// <returns>Return the current <see cref="StandardException"/> instance for supporting fluent API feature.</returns>
        public StandardException Append(IEnumerable<IStandardError> details)
        {
            if (details != null)
            {
                _details.AddRange(details);
            }

            return this;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(_message))
            {
                return base.ToString();
            }

            if (_details.Count == 0)
            {
                return $"{"{ "}\"Code\": \"{Code}\", \"Message\": \"{Message}\", \"Target\": \"{Target}\"{" }"}";
            }
            else
            {
                return $"{"{ "}\"Code\": \"{Code}\", \"Message\": \"{Message}\", \"Details\": [{string.Join(",", _details)}]{" }"}";
            }
        }
    }
}
