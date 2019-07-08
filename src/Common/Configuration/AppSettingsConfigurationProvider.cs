using Microsoft.Extensions.Configuration;

namespace Common.Configuration
{
    public class AppSettingsConfigurationProvider : IProvideConfiguration
    {
        private IConfigurationRoot _configuration;

        public IConfiguration Configuration =>_configuration ?? (_configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
    }
}