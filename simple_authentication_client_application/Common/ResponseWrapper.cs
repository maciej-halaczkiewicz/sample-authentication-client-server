namespace simple_authentication_client_application.Common;

public class ResponseWrapper
{
    public static ResponseWrapper Ok = new ResponseWrapper { IsSuccess = true };
    public static ResponseWrapper Error() => new ResponseWrapper { IsSuccess = false };

    public static ResponseWrapper Error(string errorMessage) => new ResponseWrapper { IsSuccess = false, ErrorMessage = errorMessage };

    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
}
public class ResponseWrapper<T> : ResponseWrapper
where T : class
{
    /// <summary>
    /// Public constructor for json deserialization
    /// </summary>
    public ResponseWrapper() { }
    private ResponseWrapper(T payload)
    {
        Payload = payload;
        IsSuccess = true;
    }
    private ResponseWrapper(T payload, string errorMessage)
    {
        Payload = payload;
        IsSuccess = false;
        ErrorMessage = errorMessage;
    }

    public T Payload { get; set; }
    public new static ResponseWrapper<T> Ok(T t) => new ResponseWrapper<T>(t);
    public new static ResponseWrapper<T> Error(string errorMessage) => new ResponseWrapper<T>(null, errorMessage);
    public static ResponseWrapper<T> Error(T t, string errorMessage) => new ResponseWrapper<T>(t, errorMessage);
}