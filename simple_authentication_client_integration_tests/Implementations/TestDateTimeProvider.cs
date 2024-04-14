
using simple_authentication_client_application.Abstractions;

namespace simple_authentication_client_integration_tests.Implementations
{
    public class TestDateTimeProvider : IDateTimeProvider
    {
        public static DateTime GlobalNow { get; set; } = DateTime.UtcNow;
        public DateTime Now => GlobalNow;
    }
}
