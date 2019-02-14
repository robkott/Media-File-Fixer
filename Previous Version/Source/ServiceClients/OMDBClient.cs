using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using RestSharp;

namespace ServiceClients
{
    public class OMDBClient : IShowInformationClient
    {
        private const string BaseUrl = "http://www.omdbapi.com";
        private readonly string EpisodeInfoURL = "?t={ShowName}&Season={Season}&Episode={Episode}";
        private readonly string ShowInfoURL = "?i={SeriesID}";

        public Show GetShow(ShowRequest showRequest)
        {
            var request = new RestRequest()
            {
                Resource = EpisodeInfoURL
            };
            request.AddUrlSegment("ShowName", showRequest.ShowName);
            request.AddUrlSegment("Season", showRequest.SeasonNumber.ToString());
            request.AddUrlSegment("Episode", showRequest.EpisodeNumber.ToString());

            var episode = Execute<OMDBEpisode>(request);

            if (episode.Response)
            {
                request = new RestRequest()
                {
                    Resource = ShowInfoURL
                };
                request.AddUrlSegment("SeriesID", episode.SeriesID);

                var show = Execute<OMDBShow>(request);
                
                if (show.Response)
                {
                    return new Show()
                    {
                        Name = show.Title,
                        Id = show.ImdbID,
                        Episode = new Episode()
                        {
                            EpisodeNumber = episode.Episode,
                            SeasonNumber = episode.Season,
                            Title = episode.Title
                        }
                    };
                }
                else
                {
                    throw new ShowNotFoundException()
                    {
                        ShowName = showRequest.ShowName
                    };
                }
            }
            else
            {
                throw new EpisodeNotFoundException()
                {
                    Episode = showRequest.EpisodeNumber,
                    Season = showRequest.SeasonNumber,
                    ShowName = showRequest.ShowName
                };
            }
        }

        private T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient()
            {
                BaseUrl = new Uri(BaseUrl)
            };

            var response = client.Execute<T>(request);

            return response.Data;
        }
    }

    public class OMDBEpisode
    {
        public string ImdbID { get; set; }
        public string SeriesID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int Season { get; set; }
        public int Episode { get; set; }

        public bool Response { get; set; }
        public string Error { get; set; }
    }

    public class OMDBShow
    {
        public string ImdbID { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }

        public bool Response { get; set; }
        public string Error { get; set; }
    }
}
