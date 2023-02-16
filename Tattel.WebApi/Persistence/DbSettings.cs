using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Tattel.WebApi.Persistence
{
    public class DbSettings
    {
        private IHostBuilder _hostBuilder;

        public DbSettings(IHostBuilder hostBuilder)
        {
            _hostBuilder = hostBuilder;
        }

        // Move values to a config file to read in via config builder
        public static string ConnectionString = "mongodb+srv://tatteladmin:tatteladmin@tattel.xcpdz.azure.mongodb.net/Tattel";
        public static string Database = "Tattel";
    }
}
