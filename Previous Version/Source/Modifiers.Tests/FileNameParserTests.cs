using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Modifiers.Tests
{
    [TestClass]
    public class FileNameParserTests
    {
        [TestMethod]
        public void ParsesUpperSENotation()
        {
            var fileNameParser = new FileNameParser();

            var fileName = "Once.Upon.a.time.S02E01.HDTV.x264-LOL.eztv.avi";

            var showRequest = fileNameParser.ParseFileName(fileName);

            Assert.IsNotNull(showRequest);
            Assert.AreEqual("Once Upon a time", showRequest.ShowName);
            Assert.AreEqual(2, showRequest.SeasonNumber);
            Assert.AreEqual(1, showRequest.EpisodeNumber);
        }

        [TestMethod]
        public void ParsesLowerSENotation()
        {
            var fileNameParser = new FileNameParser();

            var fileName = "Once.Upon.a.time.s02e01.HDTV.x264-LOL.eztv.avi";

            var showRequest = fileNameParser.ParseFileName(fileName);

            Assert.IsNotNull(showRequest);
            Assert.AreEqual("Once Upon a time", showRequest.ShowName);
            Assert.AreEqual(2, showRequest.SeasonNumber);
            Assert.AreEqual(1, showRequest.EpisodeNumber);
        }

        [TestMethod]
        public void ParsesUpperXNotation()
        {
            var fileNameParser = new FileNameParser();

            var fileName = "Once.Upon.a.time.2X01.HDTV.x264-LOL.eztv.avi";

            var showRequest = fileNameParser.ParseFileName(fileName);

            Assert.IsNotNull(showRequest);
            Assert.AreEqual("Once Upon a time", showRequest.ShowName);
            Assert.AreEqual(2, showRequest.SeasonNumber);
            Assert.AreEqual(1, showRequest.EpisodeNumber);
        }

        [TestMethod]
        public void ParsesLowerXNotation()
        {
            var fileNameParser = new FileNameParser();

            var fileName = "Once.Upon.a.time.2x01.HDTV.x264-LOL.eztv.avi";

            var showRequest = fileNameParser.ParseFileName(fileName);

            Assert.IsNotNull(showRequest);
            Assert.AreEqual("Once Upon a time", showRequest.ShowName);
            Assert.AreEqual(2, showRequest.SeasonNumber);
            Assert.AreEqual(1, showRequest.EpisodeNumber);
        }
    }
}
