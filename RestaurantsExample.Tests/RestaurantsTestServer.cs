using BddSharp;

namespace RestaurantsExample.Tests
{
    public class RestaurantsTestServer : TestServer
    {
        public RestaurantsTestServer(string port, string physicalPathCompiledApp) : base(port, physicalPathCompiledApp)
        {
            
        }
    }
}
