namespace Application.Contracts;

public interface IFileStorageService: IDisposable
{
    Task<string> SaveFileAsync(Stream stream, string absolutePath, string fileName);
    
    Task<string> SaveFileAsync(Stream fileStream, string fileName);

    Task<string> SaveFileWithRelativePathAsync(Stream stream, string relativePath, string fileName);
    
    Task<bool> DeleteFileAsync(string filePath);  // New method to delete a file
}