namespace SmartphoneStore.Platform.BlobStorage;

public interface IBlobStorage
{
    Task UploadBlobAsync(string fileName);
    Task<IEnumerable<int>> GetAllFilesNameAsync(Guid tabletId);
    Task<bool> ExistsAsync(string fileName);
    Task DeleteBlobAsync(string fileName);
}