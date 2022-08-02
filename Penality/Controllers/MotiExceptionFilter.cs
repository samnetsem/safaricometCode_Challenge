using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace MotiSectorAPI.Controllers
{
    public class MotiSectorAPIExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An unhandled exception was thrown by Web API controller."),
                ReasonPhrase = "An unhandled exception was thrown by Web API controller."
            };
            context.Response = msg;
        }
    }
}