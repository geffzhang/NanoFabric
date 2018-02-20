namespace NanoFabric.WebApi.ApiWidgets
{
    public interface IApiResult
    {
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>The status code.</value>
        int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        string Message { get; set; }
    }
}