namespace simple_authentication_client_application.Abstractions
{
    public interface IDateTimeProvider
    {
        public DateTime Now { get; }
    }
}