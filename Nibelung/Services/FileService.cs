using Microsoft.EntityFrameworkCore;
using Nibelung.Api.Services.Contracts;
using Nibelung.Domain.Models;
using Nibelung.Infrastructure.Configs;
using Nibelung.Infrastructure.Db;
using Nibelung.Infrastructure.Helpers;
using Nibelung.Infrastructure.Models;

namespace Nibelung.Api.Services
{
    public class FileService : IFileService
    {
        private readonly PgContext _db;
        private readonly IUserContext _userContext;
        public FileService(PgContext db, IUserContext userContext)
        {
            _db = db;
            _userContext = userContext;
        }

        public async Task<byte[]> GetFileDataAsync(Guid fileId)
        {

            NibelungFile? entity = await _db.Files.FirstOrDefaultAsync(x => x.Id == fileId);
            if (entity == null)
                throw new Exception("Файл не найден");

            string fullFilePath = GetFullFilePath(entity.Path);

            if (!File.Exists(fullFilePath))
                throw new Exception("Файл не найден");
            var fileLength = new FileInfo(fullFilePath).Length;
            var data = new byte[fileLength];
            using (var stream = System.IO.File.OpenRead(fullFilePath))
            {
                await stream.ReadAsync(data, 0, data.Length);
            }
            return data;
        }
        public async Task<string> Upload(IFormFile formFile)
        {
            Guid fileId = Guid.NewGuid();
            DateTime addedAt = DateTime.UtcNow;

            int userId = _userContext.User.UserId;

            NibelungFile nibelungFile = new NibelungFile()
            {
                Id = fileId,
                Name = formFile.FileName,
                Path = Path.Combine(FilesConfig.RootFolder, addedAt.ToString("yyyMM"), fileId + Path.GetExtension(formFile.FileName)),
                AddedAt = addedAt,
                UserId = userId
            };

            string fullPath = Path.Combine(AppContext.BaseDirectory, nibelungFile.Path);
            string dir = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using FileStream fs = File.Create(fullPath);

            await formFile.CopyToAsync(fs);

            try
            {
                await _db.Files.AddAsync(nibelungFile);
                await _db.SaveChangesAsync();
                return fileId.ToString();
            }
            catch (Exception ex)
            {
                File.Delete(nibelungFile.Path);
                return "Не удалось выполнить запись файла";
            }
        }

        public async Task<bool> DeleteFile(Guid fileid)
        {
            NibelungFile? file = await _db.Files.FirstOrDefaultAsync(x => x.Id == fileid);

            if (file == null)
                throw new Exception("Файл не найден");

            if (_userContext.User?.UserId != file.UserId)
                throw new Exception("Нет доступа");

            string fullFilePath = GetFullFilePath(file.Path);

            if (!File.Exists(fullFilePath))
                throw new Exception("Файл не найден");

            _db.Remove(file);
            await _db.SaveChangesAsync();
            File.Delete(fullFilePath);
            return true;
        }

        private string GetFullFilePath(string filePath) => Path.Combine(AppContext.BaseDirectory, filePath);
    }
}
