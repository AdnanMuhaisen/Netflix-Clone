using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix_Clone.Domain;

namespace Netflix_Clone.Application.Services.FileOperations
{
    public class FileManager : IFileManager
    {
        private readonly ILogger<FileManager> logger;
        private readonly IFileCompressor fileCompressor;
        private readonly IOptions<ContentMovieOptions> options;

        public FileManager(ILogger<FileManager> logger,IFileCompressor fileCompressor,IOptions<ContentMovieOptions> options)
        {
            this.logger = logger;
            this.fileCompressor = fileCompressor;
            this.options = options;
        }

        /// <summary>
        /// Get and delete the target file 
        /// </summary>
        /// <param name="DirectoryThatContainingTheFile"></param>
        /// <param name="firstTwoSegmentsOfTheTargetFileName">Represents the first two segments of the target file to delete</param>
        /// <returns>boolean value indicates if the file deleted or not</returns>
        public bool FindAndDeleteAFile(string DirectoryThatContainingTheFile, string firstTwoSegmentsOfTheTargetFileName)
        {
            logger.LogTrace("Try to get the movies directory files");

            string? fullPathOfTheTargetFileToDelete = Directory
                .GetFiles(DirectoryThatContainingTheFile)
                .FirstOrDefault(x => x.Contains(firstTwoSegmentsOfTheTargetFileName));

            if (string.IsNullOrEmpty(fullPathOfTheTargetFileToDelete) || !File.Exists(fullPathOfTheTargetFileToDelete))
            {
                logger.LogError("The file to delete : {filePath} does not exist", firstTwoSegmentsOfTheTargetFileName);

                return false;
            }

            try
            {
                File.Delete(fullPathOfTheTargetFileToDelete);

                logger.LogTrace("The file {file} is deleted", firstTwoSegmentsOfTheTargetFileName);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("An error occur when deleting the file {file} due to this exception {exMessage}", fullPathOfTheTargetFileToDelete, ex.Message);

                return false;
            }
        }


        public bool SaveTheOriginalAndCompressedContentFile(string pathOfTheAddedFileToCompressAndSave,
                    string pathOfTheTargetDirectoryToCompressTo,
                    string nameOfTheTargetCompressedFile)
        {
            logger.LogTrace($"Try to compress and save the file {nameof(nameOfTheTargetCompressedFile)}");

            try
            {
                fileCompressor.CompressAndSaveFile(
                    pathOfTheAddedFileToCompressAndSave,
                    pathOfTheTargetDirectoryToCompressTo,
                    nameOfTheTargetCompressedFile
                );

                File.Copy(sourceFileName: pathOfTheAddedFileToCompressAndSave,
                    destFileName: Path.Combine(options.Value.TargetDirectoryToSaveTo,
                    nameOfTheTargetCompressedFile) + Path.GetExtension(pathOfTheAddedFileToCompressAndSave));

                logger.LogTrace("The file {fileName} compressed and saved successfully", pathOfTheTargetDirectoryToCompressTo);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogTrace("An error when try to compress and save the file {fileName} due to this message : {message}", pathOfTheTargetDirectoryToCompressTo, ex.Message);

                return false;
            }
        }

    }
}
