using NLog;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.ModelBinding;
using System.Net.Http.Formatting;

namespace Presentation.Extends.Handlers
{
    public class ApiGlobalExceptionHandler : ExceptionHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public override void Handle(ExceptionHandlerContext context)
        {
            ApiError apiError;

            logger.Error(context.Exception);

            if(context.Exception is ApiException)
            {
                var exception = context.Exception as ApiException;
                apiError = new ApiError(exception.Message)
                {
                    Errors = exception.Errors
                };
            }
            else
            {
                var msg = context.Exception.GetBaseException().Message;
                string stack = context.Exception.StackTrace;

                apiError = new ApiError(msg)
                {
                    Detail = stack
                };
            }

            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new ObjectContent<ApiError>(apiError, new JsonMediaTypeFormatter())
            };

            context.Result = new ErrorMessageResult(context.Request, response);
        }
    }

    public class ErrorMessageResult : IHttpActionResult
    {
        private HttpRequestMessage _request;
        private HttpResponseMessage _httpResponseMessage;

        public ErrorMessageResult(HttpRequestMessage request, HttpResponseMessage httpResponseMessage)
        {
            _request = request;
            _httpResponseMessage = httpResponseMessage;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_httpResponseMessage);
        }
    }

    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public string[] Errors { get; set; }

        public ApiException(
            string message,
            int statusCode = 500,
            string[] errors = null) :
            base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }

        public ApiException(Exception ex, int statusCode = 500) : base(ex.Message)
        {
            StatusCode = statusCode;
        }
    }

    public class ApiError
    {
        public string Message { get; set; }
        public bool IsError { get; set; }
        public string Detail { get; set; }
        public string[] Errors { get; set; }

        public ApiError(string message)
        {
            Message = message;
            IsError = true;
        }

        public ApiError(ModelStateDictionary modelState)
        {
            IsError = true;
            if (modelState != null && modelState.Any(m => m.Value.Errors.Count > 0))
            {
                Message = "Please correct the specified errors and try again.";
                Errors = modelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToArray();
            }
        }
    }
}