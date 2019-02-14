using System;
using Domain;
using Interfaces;
using RestSharp;
using System.Collections.Generic;
using System.Linq;

namespace ServiceClients
{
    public class TvMazeClient : IShowInformationClient
    {
        private const string BaseUrl = "http://api.tvmaze.com";
        private readonly string EpisodeInfoURL = "singlesearch/shows?q={ShowName}&embed=episodes";
        public Show GetShow(ShowRequest showRequest)
        {
            var request = new RestRequest()
            {
                Resource = EpisodeInfoURL
            };
            request.AddUrlSegment("ShowName", showRequest.ShowName);

            var show = Execute<TvMazeShow>(request);

            if (show == null)
            {
                throw new ShowNotFoundException()
                {
                    ShowName = showRequest.ShowName
                };
            }

            var episode = show.Embedded
                .Episodes
                .FirstOrDefault(x => x.Season == showRequest.SeasonNumber && x.Number == showRequest.EpisodeNumber);

            if (episode == null)
            {
                throw new EpisodeNotFoundException()
                {
                    ShowName = showRequest.ShowName,
                    Season = showRequest.SeasonNumber,
                    Episode = showRequest.EpisodeNumber
                };
            }

            return new Show()
            {
                Id = show.Id,
                Name = show.Name,
                Link = show.Url,
                Episode = new Episode()
                {
                    Url = episode.Url,
                    EpisodeNumber = episode.Number,
                    SeasonNumber = episode.Season,
                    Title = episode.Name
                }
            };
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

    internal class TvMazeShow
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public TvMazeEpisodeList Embedded { get; set; }
    }

    internal class TvMazeEpisodeList
    {
        public List<TvMazeEpisode> Episodes { get; set; }

    }

    internal class TvMazeEpisode
    {
        public string Id { get; set; }
        public int Season { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

    }
}
