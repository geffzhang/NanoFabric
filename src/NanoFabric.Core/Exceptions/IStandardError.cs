using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Core.Exceptions
{
    /// <summary>
    /// Defines the interface(s) for invoke error.
    /// </summary>
    /// <remarks>
    /// The value for the "code" name/value pair is a language-independent string.
    /// Its value is a service-defined error code that SHOULD be human-readable.
    /// 
    /// The value for the "message" name/value pair MUST be a human-readable representation of the error.
    /// It is intended as an aid to developers and is not suitable for exposure to end users.
    /// 
    /// The value for the "target" name/value pair is the target of the particular error
    /// (e.g., the name of the property in error).
    /// 
    /// The value for the "details" name/value pair MUST be an array of JSON objects that MUST contain name/value pairs for "code"
    /// and "message," and MAY contain a name/value pair for "target," as described above.
    /// 
    /// The value for the "innererror" name/value pair MUST be an object.
    /// The contents of this object are service-defined. Services wanting to return more specific errors
    /// than the root-level code MUST do so by including a name/value pair for "code" and a nested "innererror."
    /// Each nested "innererror" object represents a higher level of detail than its parent.
    /// </remarks>
    public interface IStandardError
    {
        /// <summary>
        /// Gets or sets the code for this error.
        /// </summary>
        /// <value>
        /// The code for this error.
        /// </value>
        string Code { get; set; }
        /// <summary>
        /// Gets or sets the message for this error.
        /// </summary>
        /// <value>
        /// The message for this error.
        /// </value>
        string Message { get; set; }
        /// <summary>
        /// Gets or sets the target of the particular error (e.g., the name of the property in error).
        /// </summary>
        /// <value>The target of the particular error.</value>
        string Target { get; set; }
        /// <summary>
        /// An object containing more specific information than the current object about the error.
        /// </summary>
        /// <value>The inner error.</value>
        IStandardError InnerError { get; set; }
        /// <summary>
        /// Gets or sets the details for this error.
        /// </summary>
        /// <value>The details for this error.</value>
        IEnumerable<IStandardError> Details { get; set; }
    }
}
