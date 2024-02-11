using Netflix_Clone.Application.Services.IServices;

namespace Netflix_Clone.Application.Services.FileOperations
{
    public interface IFileCompressor : IScopedService
    {
        void CompressFileTo(string PathOfTheTargetFileToCompress, string CompressTo, string NameOfTheTargetCompressedFile);
    }
}