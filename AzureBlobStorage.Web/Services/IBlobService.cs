using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using Blob = AzureBlobStorage.Web.Models.Blob;

namespace AzureBlobStorage.Web.Services
{
    public interface IBlobService
    {
        Task<string> GetBlob(string name, string containerName);
        Task<List<string>> GetAllBlobs(string containerName);
        Task<List<Blob>> GetAllBlobsWithUri(string containerName);

        Task<bool> UploadBlob(string name, IFormFile file, string containerName, Blob blob);
        Task<bool> DeleteBlob(string name, string contianerName);
    }
}
