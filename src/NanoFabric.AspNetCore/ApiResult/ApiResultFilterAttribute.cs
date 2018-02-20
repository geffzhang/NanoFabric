using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NanoFabric.WebApi.ApiWidgets
{
    internal class ApiResultFilterAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context) {
            if (context.Result is ObjectResult) {
                // this include OkObjectResult, NotFoundObjectResult, BadRequestObjectResult, CreatedResult (lose Location)
                var objectResult = context.Result as ObjectResult;
                if (objectResult.Value == null) {
                    context.Result = new ObjectResult(ApiResult.Empty);
                }
                else if (!(objectResult.Value is IApiResult)) {
                    var apiResult = Activator.CreateInstance(
                        typeof(ApiResult<>).MakeGenericType(objectResult.DeclaredType), objectResult.Value, objectResult.StatusCode);
                    context.Result = new ObjectResult(apiResult);
                }
            }
            else if (context.Result is EmptyResult) {
                // return void or Task
                context.Result = new ObjectResult(ApiResult.Empty);
            }
            else if (context.Result is ContentResult) {
                context.Result = new ObjectResult(ApiResult.Succeed((context.Result as ContentResult).Content));
            }
            else if (context.Result is StatusCodeResult) {
                // this include OKResult, NoContentResult, UnauthorizedResult, NotFoundResult, BadRequestResult
                context.Result = new ObjectResult(ApiResult.From((context.Result as StatusCodeResult).StatusCode));
            }
        }
    }
}
