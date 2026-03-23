using System.Net;
using MPTeste.API.Exceptions;

namespace MPTeste.API.Middleware
{
    /// <summary>
    /// Middleware para tratamento global de exceções.
    /// Converte exceções em respostas HTTP apropriadas.
    /// </summary>
    public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, logger);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            logger.LogError(exception, "Uma exceção não tratada ocorreu: {Message}", exception.Message);

            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new ErrorResponse();

            switch (exception)
            {
                case NotFoundException notFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = notFoundEx.Message;
                    errorResponse.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case ValidationException validationEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = validationEx.Message;
                    errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case BusinessRuleViolationException businessEx:
                    response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    errorResponse.Message = businessEx.Message;
                    errorResponse.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "Ocorreu um erro interno no servidor. Tente novamente mais tarde.";
                    errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            return response.WriteAsJsonAsync(errorResponse);
        }
    }

    /// <summary>
    /// Modelo padrão de resposta de erro.
    /// </summary>
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
