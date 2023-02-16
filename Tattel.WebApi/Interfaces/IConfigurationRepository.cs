using System;
using Azure.Storage;
using Azure.Storage.Blobs;

namespace Tattel.WebApi.Interfaces
{
    public class IConfigurationRepository
    { 
         public string StorageConnectionString { get; set; }
         public string StorageContainerName { get; set; }
         public string StorageAccountName { get; set; }
         public string StorageAccountKey { get; set; }
         public BlobContainerClient Container { get; set; }
         public StorageSharedKeyCredential Key { get; set; }
         public string BaseUrl { get; set; }
         public string DatabaseConnectionString { get; set; }
         public string Database { get; set; }
           
    }
}
