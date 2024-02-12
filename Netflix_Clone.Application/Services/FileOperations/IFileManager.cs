using Netflix_Clone.Application.Services.IServices;

namespace Netflix_Clone.Application.Services.FileOperations
{
    public interface IFileManager : IScopedService
    {
        bool FindAndDeleteAFile(string DirectoryThatContainingTheFile, string targetFileNameToDelete);
        bool SaveTheOriginalAndCompressedContentFile(string pathOfTheAddedFileToCompressAndSave,
                    string pathOfTheTargetDirectoryToCompressTo,
                    string nameOfTheTargetCompressedFile);
    }
}
