using System;
using Domain;
using Interfaces;
using RestSharp;

namespace ServiceClients
{
    public class TvRageClient : IShowInformationClient
    {
        private const string BaseUrl = "http://services.tvrage.com/feeds";
        private readonly string EpisodeInfoURL = "episodeinfo.php?show={ShowName}&exact=0&ep={SeasonNumber}x{EpisodeNumber}";

        public Show GetShow(ShowRequest showRequest)
        {
            var request = new RestRequest()
                {
                    Resource = EpisodeInfoURL
                };
            request.AddUrlSegment("ShowName", showRequest.ShowName);
            request.AddUrlSegment("SeasonNumber", showRequest.SeasonNumber.ToString());
            request.AddUrlSegment("EpisodeNumber", showRequest.EpisodeNumber.ToString());

            var show = Execute<Show>(request);

            if (show == null)
            {
                throw new Exception("Could not get show data");
            }

            if (show.Episode == null)
            {
                throw new EpisodeNotFoundException()
                    {
                        ShowName = showRequest.ShowName,
                        Season = showRequest.SeasonNumber,
                        Episode = showRequest.EpisodeNumber
                    };
            }

            show.Episode.SeasonNumber = showRequest.SeasonNumber;

            return show;
        }

        private T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient()
                {
                    BaseUrl = new Uri(BaseUrl)
                };

            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                if (response.Content.Contains("No Show Results"))
                {

                    var split = response.Content.Split('"');
                    throw new ShowNotFoundException()
                        {
                            ShowName = split[1]
                        };
                }
                else
                {
                    throw response.ErrorException;
                }
            }
            return response.Data;
        }
    }
}
