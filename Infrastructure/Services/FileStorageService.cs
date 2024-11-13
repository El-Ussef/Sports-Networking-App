using System.IO.Compression;
using Application.Common.Extensions;
using Application.Contracts;

namespace Infrastructure.Identity.Services;

public class FileStorageService : IFileStorageService
{
    private readonly string _storageDirectory;

    public FileStorageService(string storageDirectory)
    {
        _storageDirectory = storageDirectory;

        // Ensure the storage directory exists
        if (!Directory.Exists(_storageDirectory))
        {
            Directory.CreateDirectory(_storageDirectory);
        }
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName)
    {
        if (fileStream == null || string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Invalid file stream or file name.");
        }

        var filePath = Path.Combine(_storageDirectory, fileName);
        using (var fileStreamOutput = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(fileStreamOutput);
        }
        
        // this might be a URL if you're using cloud storage
        return filePath;
    }
    
    public Task<string> SaveFileAsync(Stream stream, string absolutePath, string fileName)
    {
        if (!Directory.Exists(absolutePath.Trim())) Directory.CreateDirectory(absolutePath.Trim());

        using (var fileStream = File.Create(absolutePath.Trim().AddSuffix(fileName.Trim())))
        {
            stream.Position = 0;

            stream.CopyTo(fileStream);

            return Task.FromResult(absolutePath.Trim().AddSuffix(fileName.Trim()));
        }
    }
    
    public async Task<string> SaveFileWithRelativePathAsync(Stream stream, string relativePath, string fileName)
    {
        if (stream == null || string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Invalid file stream or file name.");
        }
        
        var uri = Path.Combine(_storageDirectory, relativePath);    
        var absolutePath = Path.Combine(uri, fileName);

        // Use directoryPath for directory operations
        var directoryPath = Path.GetDirectoryName(absolutePath);

        if (!Directory.Exists(directoryPath)) 
        {
            Directory.CreateDirectory(directoryPath);
        }

        try
        {
            using (var fileStreamOutput = new FileStream(absolutePath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStreamOutput);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        

        return absolutePath;
        // using (var fileStream = File.Create(absolutePath.Trim().AddSuffix(fileName.Trim())))
        // {
        //     stream.Position = 0;
        //
        //     stream.CopyTo(fileStream);
        //
        //     return Task.FromResult(absolutePath.Trim().AddSuffix(fileName.Trim()));
        // }
    }
    public async Task<bool> DeleteFileAsync(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting file {filePath}: {ex.Message}");
            return false;
        }
    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

//using FluentFTP;

// public class FTPFileStorageService : IFileStorageService
// {
//     private readonly FTPSettings _ftpSettings;
//
//     public FTPFileStorageService(IOptions<FileStorageSettings> settings)
//     {
//         _ftpSettings = settings.Value.FTP;
//     }
//
//     public async Task<string> UploadFileAsync(IFormFile file, string directory)
//     {
//         var fileName = $"{_ftpSettings.BaseDirectory}/{directory}/{Guid.NewGuid()}_{file.FileName}";
//
//         using var ftpClient = new FtpClient(_ftpSettings.Server, _ftpSettings.Username, _ftpSettings.Password);
//         ftpClient.Connect();
//
//         using var stream = file.OpenReadStream();
//         await ftpClient.UploadAsync(stream, fileName);
//
//         return GetFileUrl(fileName, directory).Result;
//     }
//
//     public Task<string> GetFileUrl(string fileName, string directory)
//     {
//         return Task.FromResult($"ftp://{_ftpSettings.Server}/{fileName}");
//     }
// }
