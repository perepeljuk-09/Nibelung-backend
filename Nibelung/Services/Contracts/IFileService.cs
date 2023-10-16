namespace Nibelung.Api.Services.Contracts
{
    public interface IFileService
    {
        public Task<byte[]> GetFileDataAsync(Guid fileId);
        public Task<string> Upload(IFormFile formFile);
        public Task<bool> DeleteFile(Guid fileid);
    }
}
