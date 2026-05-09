namespace BusesControl.Export.Core.Interfaces
{
    public interface IStorageRepository
    {
        Task<string> Upload(string fileName, string contentType, byte[] file);
    }
}
