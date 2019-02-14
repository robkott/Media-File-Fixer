using System;
using System.Text.RegularExpressions;
using Domain;
using Interfaces;

namespace Modifiers
{
    public class FileNameParser : IFileNameParser
    {
        #region Implementation of IFileNameParser

        private const string SENotationRegex = @"[S][0-9]{1,2}[E][0-9]{1,2}";
        private const string XNotationRegex = @"[0-9]{1,2}[X][0-9]{1,2}";

        public ShowRequest ParseFileName(string fileName)
        {
            var result = TrySENotation(fileName);
            if (result != null)
            {
                return result;
            }

            result = TryXNotation(fileName);
            if (result != null)
            {
                return result;
            }

            throw new ParseFileNameException(fileName, "Did not regonize SE or X notation");
        }

        private ShowRequest TrySENotation(string fileName)
        {
            RegexOptions myRegexOptions = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
            Regex myRegex = new Regex(SENotationRegex, myRegexOptions);

            Match match = myRegex.Match(fileName);
            if (match.Success == false)
            {
                return null;
            }

            int season, episode;

            var matchSplits = match.Value.ToLowerInvariant().Split('e');
            if (int.TryParse(matchSplits[0].Substring(1), out season) == false)
            {
                throw new ParseFileNameException(fileName, "Could not parse season number");
            }
            if (int.TryParse(matchSplits[1], out episode) == false)
            {
                throw new ParseFileNameException(fileName, "Could not parse episode number");
            }

            var splits = myRegex.Split(fileName);
            if (splits.Length == 0)
            {
                throw new ParseFileNameException(fileName, "Could not get show name");
            }

            var showName = splits[0].Replace('.', ' ').Trim();

            return new ShowRequest()
                {
                    ShowName = showName,
                    SeasonNumber = season,
                    EpisodeNumber = episode
                };
        }

        private ShowRequest TryXNotation(string fileName)
        {
            RegexOptions myRegexOptions = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
            Regex myRegex = new Regex(XNotationRegex, myRegexOptions);

            Match match = myRegex.Match(fileName);
            if (match.Success == false)
            {
                return null;
            }

            int season, episode;

            var matchSplits = match.Value.ToLowerInvariant().Split('x');
            if (int.TryParse(matchSplits[0], out season) == false)
            {
                throw new ParseFileNameException(fileName, "Could not parse season number");
            }
            if (int.TryParse(matchSplits[1], out episode) == false)
            {
                throw new ParseFileNameException(fileName, "Could not parse episode number");
            }

            var splits = myRegex.Split(fileName);
            if (splits.Length == 0)
            {
                throw new ParseFileNameException(fileName, "Could not get show name");
            }

            var showName = splits[0].Replace('.', ' ').Trim();

            return new ShowRequest()
            {
                ShowName = showName,
                SeasonNumber = season,
                EpisodeNumber = episode
            };
        }

        #endregion
    }
}