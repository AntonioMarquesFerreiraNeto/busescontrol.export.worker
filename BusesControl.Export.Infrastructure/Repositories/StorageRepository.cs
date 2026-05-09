using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BusesControl.Export.Core.Interfaces;
using BusesControl.Export.Domain;
using Microsoft.Extensions.Options;

namespace BusesControl.Export.Infrastructure.Repositories
{
    public class StorageRepository : IStorageRepository
    {
        public StorageRepository(IOptions<Settings> options)
        {
            _settings = options.Value;
        }

        private readonly Settings _settings;

        public async Task<string> Upload(string fileName, string contentType, byte[] file)
        {
            var blobService = new BlobServiceClient(_settings.Azure.Storage.ConnectionString);

            var containerClient = blobService.GetBlobContainerClient(_settings.Azure.Storage.ContainerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(fileName);

            using var stream = new MemoryStream(file);

            var options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
            };

            await blobClient.UploadAsync(stream, options);

            return fileName;
        }
    }
}
