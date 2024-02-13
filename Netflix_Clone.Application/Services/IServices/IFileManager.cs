namespace Netflix_Clone.Application.Services.IServices
{
    public interface IFileManager : IScopedService
    {
        bool FindAndDeleteAFile(string DirectoryThatContainingTheFile, string targetFileNameToDelete);
        bool SaveTheOriginalAndCompressedContentFile(string pathOfTheAddedFileToCompressAndSave,
                    string pathOfTheTargetDirectoryToCompressTo,
                    string nameOfTheTargetCompressedFile);
    }
}
