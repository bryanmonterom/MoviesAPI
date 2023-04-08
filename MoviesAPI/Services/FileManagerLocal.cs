namespace MoviesAPI.Services
{
    public class FileManagerLocal : IFileManager
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IHttpContextAccessor httpContext;

        public FileManagerLocal(IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContext)
        {
            this.hostEnvironment = hostEnvironment;
            this.httpContext = httpContext;
        }

        public Task DeleteFile(string path, string container)
        {
            if (path is not null) { 
            
                var fileName = Path.GetFileName(path);
                var directory = Path.Combine(hostEnvironment.WebRootPath, container, fileName);

                if (File.Exists(directory))
                {

                    File.Delete(directory);
                }
            }

            return Task.FromResult(0);
        }

        public async Task<string> EditFile(byte[] content, string fileExtension, string container, string contentType, string path)
        {
            await DeleteFile(path, container);
            return await SaveFile(content, fileExtension, container, contentType);
        }

        public async Task<string> SaveFile(byte[] content, string fileExtension, string container, string contentType)
        {
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            string folder = Path.Combine(hostEnvironment.WebRootPath, container);

            if (!Directory.Exists(folder)){ 
                Directory.CreateDirectory(folder);
            }

            string path = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(path, content);
            var currentPath = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host}";
            var finalURL = Path.Combine(currentPath, container, fileName).Replace("\\", "/");   
            return finalURL;
        }
    }
}
