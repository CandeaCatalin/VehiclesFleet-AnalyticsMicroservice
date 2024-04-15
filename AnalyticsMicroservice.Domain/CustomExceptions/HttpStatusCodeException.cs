using System.Net;

namespace AnalyticsMicroservice.Domain.CustomExceptions;

[Serializable]
public class HttpStatusCodeException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
    public string DisplayMessage { get; set; }

    public HttpStatusCodeException(HttpStatusCode statusCode, string exceptionMessage, string displayMessage = null) :
        base(exceptionMessage)
    {
        StatusCode = statusCode;
        DisplayMessage = displayMessage ?? exceptionMessage;
    }
}