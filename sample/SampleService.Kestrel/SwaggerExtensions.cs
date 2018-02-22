using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleService.Kestrel
{
    /// <summary>
    /// 添加控制器模块说明
    /// </summary>
    public class ApplyTagDescriptions : IDocumentFilter
    {
        /// <summary>
        /// apply
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new List<Tag>
            {
                new Tag { Name = "Status", Description = "健康检查" },
                new Tag { Name = "Values", Description = "Values测试服务接口" },
            };
        }
    }
    /// <summary>
    /// 操作过过滤器 添加通用参数等
    /// </summary>
    public class AssignOperationVendorExtensions : IOperationFilter
    {
        /// <summary>
        /// apply
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {  
            var fileAttr = context.ApiDescription.ActionAttributes().OfType<SwaggerFileUploadAttribute>().FirstOrDefault();
            if (fileAttr != null)
            {
                //operation.Parameters.Add("multipart/form-data");
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "file",
                    In = "formData",
                    Description = "上传图片",
                    Required = (fileAttr as SwaggerFileUploadAttribute).Required,
                    Type = "file"
                });

            }
        }
    }
    /// <summary>
    /// 文件上传
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SwaggerFileUploadAttribute : Attribute
    {
        public bool Required { get; private set; }

        public SwaggerFileUploadAttribute(bool Required = true)
        {
            this.Required = Required;
        }
    }
}
