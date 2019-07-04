using Microsoft.Extensions.Configuration;

namespace Common.Configuration
{
    public interface IProvideConfiguration
    {
        IConfiguration Configuration { get; }
    }
}
