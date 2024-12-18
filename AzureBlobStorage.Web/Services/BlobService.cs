
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Reflection.Metadata;
using AzureBlobStorage.Web.Models;
using Blob = AzureBlobStorage.Web.Models.Blob;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
namespace AzureBlobStorage.Web.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        public async Task<bool> DeleteBlob(string name, string contianerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(contianerName);
            var blobClient = blobContainerClient.GetBlobClient(name);
            return await blobClient.DeleteIfExistsAsync();
        }
        public async Task<List<string>> GetAllBlobs(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobsAsync();
            var blobString = new List<string>();
            await foreach (var blob in blobs)
            {
                blobString.Add(blob.Name);

            }
            return blobString;
        }

        public async Task<List<Blob>> GetAllBlobsWithUri(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobsAsync();
            var blobList = new List<Blob>();
            await foreach (var item in blobs)
            {
                var blobClient = blobContainerClient.GetBlobClient(item.Name);

                Blob blobIndividual = new()
                {
                    Uri = blobClient.Uri.AbsoluteUri
                };
                if (blobClient.CanGenerateSasUri)
                {
                    BlobSasBuilder sasBuilder = new()
                    {
                        BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                        BlobName = blobClient.Name,
                        Resource = "b",
                        ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
                    };
                    sasBuilder.SetPermissions(BlobSasPermissions.Read);
                    blobIndividual.Uri = blobClient.GenerateSasUri(sasBuilder).AbsoluteUri;
                }
                BlobProperties properties = await blobClient.GetPropertiesAsync();
                if (properties.Metadata.ContainsKey("title"))
                {
                    blobIndividual.Title = properties.Metadata["title"];
                }
                if (properties.Metadata.ContainsKey("comment"))
                {
                    blobIndividual.Comment = properties.Metadata["comment"];
                }
                blobList.Add(blobIndividual);
            }
            return blobList;
        }
        public async Task<string> GetBlob(string name, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(name);
            return blobClient.Uri.AbsoluteUri;
        }
        public async Task<bool> UploadBlob(string name, IFormFile file, string containerName, Blob blob)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(name);
            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };
            IDictionary<string, string> metadata =
             new Dictionary<string, string>();
            metadata.Add("title", blob.Title);
            metadata["comment"] = blob.Comment;
            var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders, metadata);

            if (result != null)
            {
                return true;
            }
            return false;
        }
    }
}
