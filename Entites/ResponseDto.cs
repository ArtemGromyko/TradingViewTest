using System.Net;

namespace Entites;

public class ResponseDto
{
    public string Symbol { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}
