using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace API.OperationFilters
{
    public class AuthorizeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.GetCustomAttributes().OfType<AuthorizeAttribute>().Any() || context.MethodInfo.DeclaringType != null && context.MethodInfo.DeclaringType.GetCustomAttributes().OfType<AuthorizeAttribute>().Any())
            {
                operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(), new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add(StatusCodes.Status403Forbidden.ToString(), new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "oauth2"
                                }
                            },
                            new List<string>
                            {
                                "weatherApi"
                            }
                        }
                    }
                };
            }

            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "ClientId",
                In = ParameterLocation.Header,
                Required = true // set to false if this is optional
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "ClientSecret",
                In = ParameterLocation.Header,
                Required = true // set to false if this is optional
            });
        }
    }
}
