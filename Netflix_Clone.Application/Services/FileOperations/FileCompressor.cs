using Netflix_Clone.Domain.Enums;
using System.IO.Compression;
using Serilog;
using Microsoft.Extensions.Logging;

namespace Netflix_Clone.Application.Services.FileOperations
{
    public class FileCompressor(ILogger<FileCompressor> logger) : IFileCompressor
    {
        private readonly ILogger<FileCompressor> logger = logger;


        /// <summary>
        /// Compress and save the file to the needed directory
        /// </summary>
        /// <param name="PathOfTheTargetFileToCompress"></param>
        /// <param name="DirectoryToCompressTo">Represents the directory that you want to save in it</param>
        /// <param name="NameOfTheTargetCompressedFile"></param>
        /// <returns></returns>
        public void CompressAndSaveFile(
            string PathOfTheTargetFileToCompress,
            string DirectoryToCompressTo,
            string NameOfTheTargetCompressedFile)
        {
            try
            {
                logger.LogTrace($"The {nameof(CompressAndSaveFile)} operation have been started");

                using FileStream originalFileStream = File.Open(PathOfTheTargetFileToCompress, FileMode.Open);

                // this creates a new compressed file
                string compressedFilePath = Path.Combine(DirectoryToCompressTo, NameOfTheTargetCompressedFile) + "." +
                    FileExtensions.GZ.ToString().ToLower();

                using FileStream compressedFileStream = File.Create(compressedFilePath);

                using var compressor = new GZipStream(compressedFileStream, CompressionMode.Compress);

                originalFileStream.CopyTo(compressor);
            }
            catch (Exception ex)
            {
                logger.LogTrace($"An exception occur in {nameof(CompressAndSaveFile)} due to this cause : {ex.Message}");
            }
        }
    }
}
