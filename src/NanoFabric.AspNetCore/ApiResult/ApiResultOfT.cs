namespace NanoFabric.WebApi.ApiWidgets {

    public class ApiResult<TResult> : ApiResult, IApiResult<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResult{TResult}"/> class.
        /// </summary>
        public ApiResult() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResult{TResult}" /> class.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="statusCode">The status code.</param>
        public ApiResult(TResult result, int? statusCode) {
            StatusCode = statusCode ?? 200;
            Result = result;
        }
        
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        public TResult Result { get; set; }
    }
}
