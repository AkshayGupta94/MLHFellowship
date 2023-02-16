using System;
using System.IO;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Tattel.WebApi.Interfaces;

namespace Tattel.WebApi.Persistence
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        //public string StorageConnectionString { get; set; }
        //public string StorageContainerName { get; set; }
        //public string StorageAccountName { get; set; }
        //public string StorageAccountKey { get; set; }
        //public BlobContainerClient Container { get; set; }
        //public StorageSharedKeyCredential Key { get; set; }
        //public string BaseUrl { get; set; }
        //public string DatabaseConnectionString { get; set; }
        //public string Database { get; set; }

        private readonly IConfiguration _configuration;
        public ConfigurationRepository()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
            /* //UNCOMMENT for local debugging
            var jsonPath = Path.Combine(Environment.CurrentDirectory, "appsettings.json"];

            var configuration = new ConfigurationBuilder()
                                    .AddEnvironmentVariables()
                                    .AddJsonFile(jsonPath, optional: false, reloadOnChange: true)
                                    .Build(];


            StorageConnectionString = configuration.GetValue<string>("StorageAccount:StorageAccountConnectionString"];
            StorageContainerName = configuration.GetValue<string>("StorageAccount:StorageContainer"];
            StorageAccountName = configuration.GetValue<string>("StorageAccount:StorageAccountNAme"];
            StorageAccountKey = configuration.GetValue<string>("StorageAccount:StorageAccountKey"];
            Container = new BlobContainerClient(StorageConnectionString, StorageContainerName];
            Key = new StorageSharedKeyCredential(StorageAccountName, StorageAccountKey];
            BaseUrl = configuration.GetValue<string>("Urls:BaseUrl"];
            DatabaseConnectionString = configuration.GetValue<string>("DatabaseConnection:ConnectionString"];
            Database = configuration.GetValue<string>("DatabaseConnection:Database"];
            */


            // Reading in values from Azure App Settings for Web Api
            StorageConnectionString = _configuration["StorageAccount:StorageAccountConnectionString"];
            StorageContainerName = _configuration["StorageAccount:StorageContainer"];
            StorageAccountName =_configuration["StorageAccount:StorageAccountName"];
            StorageAccountKey =_configuration["StorageAccount:StorageAccountKey"];
            Container = new BlobContainerClient(StorageConnectionString, StorageContainerName);
            Key = new StorageSharedKeyCredential(StorageAccountName, StorageAccountKey);
            BaseUrl =_configuration["Urls:BaseUrl"];
            DatabaseConnectionString =_configuration["DatabaseConnection:ConnectionString"];
            Database =_configuration["DatabaseConnection:Database"];
            
        }
    }
}
