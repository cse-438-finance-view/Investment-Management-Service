using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace InvestmentManagementService.Infrastructure.Swagger
{
    public class DateOnlySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(DateOnly) || context.Type == typeof(DateOnly?))
            {
                schema.Type = "string";
                schema.Format = "date";
                schema.Example = OpenApiAnyFactory.CreateFromJson("\"2000-01-01\"");
                schema.Pattern = "^\\d{4}-\\d{2}-\\d{2}$";
            }
        }
    }
} 