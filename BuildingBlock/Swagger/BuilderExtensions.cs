using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Swagger
{
    public static class BuilderExtensions
    {
        public static void AddCustomSwagger(this WebApplicationBuilder builder, string xmlPath = "")
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.FullName);
                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new  OpenApiSecurityScheme
                    {
                        Reference = new  OpenApiReference { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "bearerAuth" }
                    },
                    new string[] {}
                }
            });

                if (!string.IsNullOrEmpty(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

            });
        }

        public static void UseCustomSwagger(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (true || app.Environment.IsDevelopment())
            {
                //app.UseSwagger(); 
                app.UseSwagger(options => options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);
                app.UseSwaggerUI(c =>
                {
                    c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
                });
            }
        }
    }
}
