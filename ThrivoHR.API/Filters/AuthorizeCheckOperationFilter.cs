﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ThrivoHR.API.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!HasAuthorize(context))
            {
                return;
            }
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                }] = []
            });
        }

        private static bool HasAuthorize(OperationFilterContext context)
        {
            if (context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any())
            {
                return true;
            }
            return context.MethodInfo.DeclaringType != null
                && context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
        }
    }
}