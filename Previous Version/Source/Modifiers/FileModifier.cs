using System;
using System.IO;
using System.Linq;
using System.Text;
using Domain;
using Domain.Extensions;
using Interfaces;
using log4net;

namespace Modifiers
{
    public interface IFileModifier : IModifier
    {

    }

    public class FileModifier : IFileModifier
    {
        private readonly IFileNameParser _fileNameParser;
        private readonly IShowInformationClient _showInformationClient;
        private readonly ISettingsManager _settingsManager;

        public FileModifier(IFileNameParser fileNameParser,
                            IShowInformationClient showInformationClient,
                            ISettingsManager settingsManager)
        {
            _fileNameParser = fileNameParser;
            _showInformationClient = showInformationClient;
            _settingsManager = settingsManager;

            log4net.Config.XmlConfigurator.Configure();
        }

        public ModifyFileResult RenameAndMove(string fullyQualifiedPath)
        {
            ILog logger = LogManager.GetLogger(typeof (FileModifier));

            logger.InfoFormat("File to move: {0}", fullyQualifiedPath);

            var result = new ModifyFileResult()
                {
                    SourceFileName = fullyQualifiedPath
                };

            try
            {
                var baseTvPath = _settingsManager.BaseTvPath;
                var fileName = Path.GetFileNameWithoutExtension(fullyQualifiedPath);
                var originalExtension = Path.GetExtension(fullyQualifiedPath);

                var showRequest = _fileNameParser.ParseFileName(fileName);

                logger.InfoFormat("Show Request: Name: {0}, Season: {1}, Episode: {2}",
                                  showRequest.ShowName,
                                  showRequest.SeasonNumber,
                                  showRequest.EpisodeNumber);

                var showInformation = _showInformationClient.GetShow(showRequest);

                logger.InfoFormat("Show Response: Name: {0}, Episode Number: {1}, Episode Name: {2}",
                                  showInformation.Name,
                                  showInformation.Episode.EpisodeNumber,
                                  showInformation.Episode.Title);

                var revisedShowName = CleanFileName(ReviseShowName(showInformation.Name));
                var revisedEpisodeName = CleanFileName(showInformation.Episode.Title);
                var paddedSeason = PadLeadingZero(showInformation.Episode.SeasonNumber);
                var outputFolder = Path.Combine(baseTvPath, revisedShowName, paddedSeason);
                var newFileName = string.Format("{0}. {1}{2}",
                                                PadLeadingZero(showInformation.Episode.EpisodeNumber),
                                                revisedEpisodeName,
                                                originalExtension);

                var outputFile = Path.Combine(outputFolder, newFileName);

                logger.InfoFormat("Target File: {0}", outputFile);

                result.DestinationFileName = outputFile;

                Directory.CreateDirectory(outputFolder);

                File.Move(fullyQualifiedPath, outputFile);

                result.Success = true;
            }
            catch (ShowNotFoundException ex)
            {
                logger.Error("Could not get show information");
                logger.ErrorFormat("Show Name: {0}",
                                   ex.ShowName);
                logger.Error(ex.BuildErrorMessage());

                result.Success = false;
                result.Exception = ex;
            }
            catch (EpisodeNotFoundException ex)
            {
                logger.Error("Could not get episode information");
                logger.ErrorFormat("Show Name: {0}, Season Number: {1}, Episode Number: {2}",
                                   ex.ShowName,
                                   ex.Season,
                                   ex.Episode);
                logger.Error(ex.BuildErrorMessage());

                result.Success = false;
                result.Exception = ex;
            }
            catch (Exception ex)
            {
                logger.Error("There was an error processing the file:");
                logger.Error(ex.BuildErrorMessage());

                result.Success = false;
                result.Exception = ex;
            }

            return result;
        }

        private string PadLeadingZero(int i)
        {
            return i.ToString("D2");
        }

        private string ReviseShowName(string showName)
        {
            var lower = showName.ToLower();
            string result = showName;

            if (lower.StartsWith("the"))
            {
                result = showName.Substring(3).Trim();
            }

            return result;
        }

        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }
    }
}
