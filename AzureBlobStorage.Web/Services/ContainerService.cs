
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobStorage.Web.Services
{
    public class ContainerService : IContainerService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public ContainerService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task CreateContainer(string contianerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(contianerName);
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.None);

        }

        public async Task DeleteContainer(string contianerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(contianerName);
            await blobContainerClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllContainer()
        {
            List<string> containerNames = new List<string>();
            await foreach (BlobContainerItem blobContainerItem in _blobServiceClient.GetBlobContainersAsync())
            {
                containerNames.Add(blobContainerItem.Name);
            }
            return containerNames;
        }

        public async Task<List<string>> GetAllContainerAndBlobs()
        {
            List<string> containerAndBlobNames = new();
            containerAndBlobNames.Add("Account Name : " + _blobServiceClient.AccountName);
            containerAndBlobNames.Add("------------------------------------------------------------------------------------------------------------");
            await foreach (BlobContainerItem blobContainerItem in _blobServiceClient.GetBlobContainersAsync())
            {
                containerAndBlobNames.Add("--" + blobContainerItem.Name);
                BlobContainerClient _blobContainer =
                      _blobServiceClient.GetBlobContainerClient(blobContainerItem.Name);
                await foreach (BlobItem blobItem in _blobContainer.GetBlobsAsync())
                {
                    containerAndBlobNames.Add("------" + blobItem.Name);
                    //get metadata
                    var blobClient = _blobContainer.GetBlobClient(blobItem.Name);
                    BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
                    string blobToAdd = blobItem.Name;
                    if (blobProperties.Metadata.ContainsKey("title"))
                    {
                        blobToAdd += "(" + blobProperties.Metadata["title"] + ")";
                    }
                    containerAndBlobNames.Add("------" + blobToAdd);
                }
                containerAndBlobNames.Add("------------------------------------------------------------------------------------------------------------");
            }
            return containerAndBlobNames;
        }
    }
}
