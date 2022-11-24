namespace Entites.Exceptions;

public class IexCloudException : Exception
{
    public IexCloudErrorCodes ErrorCode { get; set; }

    public IexCloudException(string message, HttpResponseMessage response) : base(message)
    {
        ErrorCode = (IexCloudErrorCodes)response.StatusCode;
    }

    public IexCloudException(HttpResponseMessage response) : base("Iex Cloud Error")
    {
        ErrorCode = (IexCloudErrorCodes)response.StatusCode;
    }
}
