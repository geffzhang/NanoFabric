namespace NanoFabric.WebApi.ApiWidgets
{
    public interface IApiResult<TResult> : IApiResult {
        
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>The result.</value>
        TResult Result { get; set; }
    }
}
