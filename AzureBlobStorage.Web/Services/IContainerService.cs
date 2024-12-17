namespace AzureBlobStorage.Web.Services
{
    public interface IContainerService
    {
        Task<List<string>> GetAllContainerAndBlobs();
        Task<List<string>> GetAllContainer();
        Task CreateContainer(string contianerName);
        Task DeleteContainer(string contianerName);
    }
}
