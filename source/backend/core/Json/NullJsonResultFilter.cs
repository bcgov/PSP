using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Pims.Core.Json
{
    public class NullJsonResultFilter : IResultFilter
    {

        private readonly IOptions<JsonOptions> _jsonSerializerOptions;

        public NullJsonResultFilter(IOptions<JsonOptions> jsonSerializerOptions)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is JsonResult jsonResult)
            {
                JsonSerializerOptions options = new JsonSerializerOptions(_jsonSerializerOptions.Value.JsonSerializerOptions);
                options.DefaultIgnoreCondition = JsonIgnoreCondition.Never;

                jsonResult.SerializerSettings = options;
            }
        }
    }
}
