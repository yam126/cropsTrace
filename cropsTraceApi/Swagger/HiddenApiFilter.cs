﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace cropsTraceApi.Swagger
{
    public class HiddenApiFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (ApiDescription apiDescription in context.ApiDescriptions)
            {
                if (apiDescription.TryGetMethodInfo(out MethodInfo method))
                {
                    if (method.ReflectedType.CustomAttributes.Any(t => t.AttributeType == typeof(HiddenApiAttribute))
                            || method.CustomAttributes.Any(t => t.AttributeType == typeof(HiddenApiAttribute)))
                    {
                        string key = "/" + apiDescription.RelativePath;
                        if (key.Contains("?"))
                        {
                            int idx = key.IndexOf("?", System.StringComparison.Ordinal);
                            key = key.Substring(0, idx);
                        }
                        swaggerDoc.Paths.Remove(key);
                    }
                }
            }
        }
    }
}
