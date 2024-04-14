using simple_authentication_client_application.Abstractions;

namespace simple_authentication_client_infrastructure
{
    public class DateTimeProvider:IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}