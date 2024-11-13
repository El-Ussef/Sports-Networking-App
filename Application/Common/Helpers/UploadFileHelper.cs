using Application.Common.Models;
using Application.Contracts;
using Domain.Enums;

namespace Application.Common.Helpers;

public class UploadFileHelper
{
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUserService _currentUserService;
    
    public UploadFileHelper(IFileStorageService fileStorageService, ICurrentUserService currentUserService)
    {
        _fileStorageService = fileStorageService;
        _currentUserService = currentUserService;
    }
    
    public async Task<string> SaveProfilePhotoAsync(FileUpload fileUpload)
    {
        string userType = !string.IsNullOrEmpty(_currentUserService.UserType)
            ? _currentUserService.UserType
            : UserType.BaseType.ToString();
        
        var path = Path.Combine(_currentUserService.UserType, _currentUserService.UserId);
            
        var filePath = await _fileStorageService.SaveFileWithRelativePathAsync(fileUpload.Data,path, fileUpload.FileName);
            
        // Extract the filename from the filePath
        var fileName = Path.GetFileName(filePath);

        // Return a relative URL
        return $"Images/{path}/{fileName}";
        return filePath;
    }
    
    public async Task<bool> DeleteProfilePhotoAsync(string fileName)
    {
        string userType = !string.IsNullOrEmpty(_currentUserService.UserType)
            ? _currentUserService.UserType
            : UserType.BaseType.ToString();
        
        var path = Path.Combine(userType, _currentUserService.UserId, fileName);

        return await _fileStorageService.DeleteFileAsync(path);
    }

    public async Task<bool> DeleteFilesAsync(List<string> fileNames)
    {
        string userType = !string.IsNullOrEmpty(_currentUserService.UserType)
            ? _currentUserService.UserType
            : UserType.BaseType.ToString();

        bool allDeleted = true;

        foreach (var fileName in fileNames)
        {
            var path = Path.Combine(userType, _currentUserService.UserId, fileName);
            var result = await _fileStorageService.DeleteFileAsync(path);
            if (!result)
            {
                allDeleted = false;
            }
        }

        return allDeleted;
    }
    public async Task<bool> DeleteFileAsync(string fileName)
    {
        string userType = !string.IsNullOrEmpty(_currentUserService.UserType)
            ? _currentUserService.UserType
            : UserType.BaseType.ToString();

        bool allDeleted = true;
        var path = Path.Combine(userType, _currentUserService.UserId, fileName);
        var result = await _fileStorageService.DeleteFileAsync(path);
        if (!result)
        {
            allDeleted = false;
        }
        return allDeleted;
    }
}