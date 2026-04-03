using AuthService.Services.Interfaces;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AuthService.Services.Implementations
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        private const long MaxImageSize = 5 * 1024 * 1024;
        private const long MaxVideoSize = 50 * 1024 * 1024;

        private static readonly string[] AllowedImageTypes = { ".jpg", ".jpeg", ".png", "webp", };
        private static readonly string[] AllowedVideoTypes = { ".mp4", ".mov", ".webm", ".gif" };
        public BlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            var containerName = configuration["AzureBlobStorage:ContainerName"];

            _containerClient = new BlobContainerClient(connectionString, containerName);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string fileCategory)
        {
            ValidateFile(file, fileCategory);
            var fileName = $"{Guid.NewGuid()} {Path.GetExtension(file.FileName)}";
            var blobClient = _containerClient.GetBlobClient(fileName);

            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            };

            var transferOptions = new StorageTransferOptions
            {
                MaximumTransferSize = 1 * 1024 * 1024,
                MaximumConcurrency = 4,
                InitialTransferSize = 1 * 1024 * 1024
            };

            var uploadOptions = new BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders,
                TransferOptions = transferOptions
            };

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, uploadOptions);

            return blobClient.Uri.ToString();
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            var blobName = ExtractBlobNameFromUrl(fileUrl);
            if (string.IsNullOrEmpty(blobName))
                return;
            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }

        private void ValidateFile(IFormFile file, string fileCategory)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (fileCategory == "image")
            {
                if (!AllowedImageTypes.Contains(extension))
                    throw new InvalidOperationException(
                        $"Invalid image format: {string.Join(", ", AllowedImageTypes)}");
                if (file.Length > MaxImageSize)
                    throw new InvalidOperationException("File is too large! Max size is 5MB");
            }
            else if (fileCategory == "video")
            {
                if (!AllowedVideoTypes.Contains(extension))
                    throw new InvalidOperationException(
                        $"Invalid video format: {string.Join(", ", AllowedVideoTypes)}");
                if (file.Length > MaxVideoSize)
                    throw new InvalidOperationException("File is too large! max is 50MB");
            }
            else
            {
                throw new InvalidOperationException("Unknown file category");
            }
        }

        private string ExtractBlobNameFromUrl(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return string.Empty;
            if (!Uri.TryCreate(fileUrl, UriKind.Absolute, out var uri))
                return string.Empty;

            return uri.Segments.Last();
        }
    }
}
