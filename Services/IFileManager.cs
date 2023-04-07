namespace MoviesAPI.Services
{
    public interface IFileManager
    {
        Task<string> SaveFile(byte[] content, string fileExtension, string container, string contentType);
        Task<string> EditFile(byte[] content, string fileExtension, string container, 
                              string contentType, string path);
        Task DeleteFile(string path, string container);
    
    }
}
