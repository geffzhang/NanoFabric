namespace NanoFabric.WebApi.ApiWidgets
{
    public class ApiResult : IApiResult {
        /// <summary>
        /// Represents an empty <see cref="IApiResult"/>.
        /// </summary>
        public static readonly IApiResult Empty = new ApiResult {
            StatusCode = 204
        };

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="IApiResult{TResult}"/> by the specified result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="result">The result.</param>
        /// <returns>An instance inherited from <see cref="IApiResult{TResult}"/> interface.</returns>
        public static IApiResult<TResult> Succeed<TResult>(TResult result) => new ApiResult<TResult> {
            StatusCode = 200,
            Result = result
        };

        /// <summary>
        ///  Creates a new instance of <see cref="IApiResult"/> by the specified error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="statusCode">The status code</param>
        /// <returns>An instance inherited from <see cref="IApiResult"/> interface.</returns>
        public static IApiResult Failed(string message, int? statusCode = null) => new ApiResult {
            StatusCode = statusCode ?? 400,
            Message = message
        };

        /// <summary>
        ///  Creates a new instance of <see cref="IApiResult{TResult}"/> by the specified error message.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="errorResult">The error result.</param>
        /// <param name="message">The message.</param>
        /// <param name="statusCode">The status code.</param>
        /// <returns>An instance inherited from <see cref="IApiResult"/> interface.</returns>
        public static IApiResult<TResult> Failed<TResult>(TResult errorResult, string message, int? statusCode = null) => new ApiResult<TResult> {
            StatusCode = statusCode ?? 400,
            Message = message,
            Result = errorResult
        };

        /// <summary>
        /// Creates a new instance of <see cref="IApiResult"/> by the specified status code and message.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="message">The message.</param>
        /// <returns>An instance inherited from <see cref="IApiResult"/> interface.</returns>
        public static IApiResult From(int statusCode, string message = null) => new ApiResult {
            StatusCode = statusCode,
            Message = message
        };

        /// <summary>
        /// Creates a new instance of <see cref="IApiResult{TResult}"/> by the specified result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="result">The result.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="message">The message.</param>
        /// <returns>An instance inherited from <see cref="IApiResult{TResult}"/> interface.</returns>
        public static IApiResult<TResult> From<TResult>(TResult result, int statusCode, string message) => new ApiResult<TResult> {
            StatusCode = statusCode,
            Message = message,
            Result = result
        };
    }
}
