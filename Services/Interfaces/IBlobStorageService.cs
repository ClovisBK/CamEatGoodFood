namespace AuthService.Services.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string fileCategory);
        Task DeleteFileAsync(string fileUrl);
    }
}
