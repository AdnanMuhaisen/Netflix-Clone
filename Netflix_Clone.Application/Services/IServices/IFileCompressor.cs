using Netflix_Clone.Application.Services.IServices;

namespace Netflix_Clone.Application.Services.FileOperations
{
    public interface IFileCompressor : IScopedService
    {
        void CompressAndSaveFile(string PathOfTheAddedFileToCompress, string CompressToDirectory, string NameOfTheTargetCompressedFile);
    }
}