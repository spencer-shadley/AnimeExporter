using AnimeExporter;
using AnimeExporterTests.utility;
using NUnit.Framework;

using static AnimeExporterTests.utility.TestConstants;

namespace AnimeExporterTests.test {
    
    /// <summary>
    /// Contains NUnit tests for <see cref="AnimeDetailsPage"/>
    /// </summary>
    public class AnimeDetailsPageTest {
        
        public Anime Anime;

        public AnimeDetailsPageTest() {
            this.Anime = new Anime();
        }

        [TestFixture]
        public class TestScraping {
            private AnimeDetailsPage DetailsPage { get; set; }

            [SetUp] public void Setup()       { this.DetailsPage = CreateData.KimiNoNaWaDetailsPage(); }
            [TearDown] public void Teardown() { this.DetailsPage = null; }

            [Test]
            public void ScrapeTitle() {
                Assert.AreEqual(this.DetailsPage.Title, KimiNoNaWa.Title);
            }
        }
    }
}
