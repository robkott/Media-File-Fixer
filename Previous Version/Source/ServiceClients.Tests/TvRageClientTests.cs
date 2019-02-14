using System;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceClients.Tests
{
    [TestClass]
    public class TvRageClientTests
    {
        private TvRageClient CreateSut()
        {
            return new TvRageClient();
        }

        [TestMethod]
        public void MatchesShowNameAndEpisode()
        {
            var sut = CreateSut();
            var show = sut.GetShow(
                new ShowRequest()
                    {
                        ShowName = "Grimm",
                        SeasonNumber = 2,
                        EpisodeNumber = 3
                    });

            Assert.IsNotNull(show);
            Assert.AreEqual("Grimm", show.Name);
            Assert.AreEqual("28352", show.Id);
            Assert.AreEqual("http://www.tvrage.com/Grimm", show.Link);

            Assert.IsNotNull(show.Episode);
            Assert.AreEqual("02x03", show.Episode.EpisodeNumber);
            Assert.AreEqual("Bad Moon Rising", show.Episode.Title);
            Assert.AreEqual("http://www.tvrage.com/Grimm/episodes/1065209088", show.Episode.Url);
        }

        [TestMethod]
        public void InvalidShowThrowsShowNotFoundException()
        {
            var showName = "lkjfsalkjas";
            var sut = CreateSut();
            AssertRaisesException<ShowNotFoundException>(() => sut.GetShow(
                new ShowRequest()
                    {
                        ShowName = showName,
                        SeasonNumber = 2,
                        EpisodeNumber = 3
                    }),
                                                         x => x.ShowName == showName);
        }

        [TestMethod]
        public void InvalidEpisodeThrowsEpisodeNotFoundException()
        {
            var sut = CreateSut();
            AssertRaisesException<EpisodeNotFoundException>(() => sut.GetShow(
                new ShowRequest()
                    {
                        ShowName = "Grimm",
                        SeasonNumber = 100,
                        EpisodeNumber = 3
                    }),
                                                            x =>
                                                            x.ShowName == "Grimm" && x.Season == 100 && x.Episode == 3);
        }


        ///<summary>
        /// Runs the action statement and asserts that it causes an exception with the expected type and message
        ///</summary>
        ///<typeparam name="TException"></typeparam>
        ///<param name="action"></param>
        ///<param name="expectedMessage"></param>
        public static void AssertRaisesException<TException>(Action action, Func<TException, bool> validator)
            where TException : Exception
        {
            try
            {
                action();
                Assert.Fail("Call succeeded. Expected exception of type: {0}", typeof(TException).Name);
            }
            catch (Exception ex)
            {
                if (ex is AssertFailedException)
                    throw ex;

                var exception = ex as TException;
                Assert.IsNotNull(exception, string.Format("Expected exception of type: {0}, actual type: {1}", typeof(TException).Name, ex.GetType().Name));
                Assert.IsTrue(validator.Invoke(exception), "Exception was not configured properly");
            }
        }
    }
}
