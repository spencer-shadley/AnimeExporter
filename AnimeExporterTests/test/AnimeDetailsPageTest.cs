using AnimeExporter;
using AnimeExporterTests.utility;
using NUnit.Framework;

using static AnimeExporterTests.utility.TestConstants;

namespace AnimeExporterTests.test {
    
    /// <summary>
    /// Contains NUnit tests for <see cref="AnimeDetailsPage"/>
    /// </summary>
    public class AnimeDetailsPageTest {

        [TestFixture]
        public class TestScraping {
            private AnimeDetailsPage KimiDetailsPage { get; set; }
            private AnimeDetailsPage SteinsGateDetailsPage { get; set; }

            [SetUp]
            public void Setup() {
                this.KimiDetailsPage = CreateData.KimiNoNaWaDetailsPage();
                this.SteinsGateDetailsPage = CreateData.SteinsGateDetailsPage();
            }

            [TearDown]
            public void Teardown() {
                this.KimiDetailsPage = null;
                this.SteinsGateDetailsPage = null;
            }

            [Test]
            public void Title() {
                Assert.That(this.KimiDetailsPage.Title, Is.EqualTo(KimiNoNaWa.Title));
            }

            [Test]
            public void Rank() {
                int rank = int.Parse(this.KimiDetailsPage.Rank); 
                
                // assuming Kimi no Na Wa will always be at ranked at least 1,000
                Assert.That(rank, Is.LessThan(1000));  
                Assert.That(rank, Is.GreaterThan(0));
            }

            [Test]
            public void MediaType() {
                Assert.That(this.KimiDetailsPage.MediaType, Is.EqualTo(KimiNoNaWa.Type));
            }

            [Test]
            public void Image() {
                Assert.That(this.KimiDetailsPage.ImageUrl, Is.EqualTo(KimiNoNaWa.ImageUrl));
            }

            [Test]
            public void Score() {
                double score = double.Parse(this.KimiDetailsPage.Score);
                Assert.That(score, Is.LessThan(9.9));
                Assert.That(score, Is.GreaterThan(8.5));
            }

            [Test]
            public void Background() {
                StringAssert.Contains(SteinsGate.PartialBackground, this.SteinsGateDetailsPage.Background);
            }

            [Test]
            public void NoBackground() {
                Assert.IsNull(this.KimiDetailsPage.Background);
            }

            [Test]
            public void EnglishTitle() {
                Assert.That(this.KimiDetailsPage.EnglishTitle, Is.EqualTo(KimiNoNaWa.EnglishTitle));
            }

            [Test]
            public void Genres() {
                Assert.That(this.KimiDetailsPage.Genres, Is.EqualTo(KimiNoNaWa.Genres));
            }

            [Test]
            public void AirStartDate() {
                Assert.That(this.SteinsGateDetailsPage.AirStartDate, Is.EqualTo(SteinsGate.AirStartDate));
            }

            [Test]
            public void AirFinishDate() {
                Assert.That(this.SteinsGateDetailsPage.AirFinishDate, Is.EqualTo(SteinsGate.AirFinishDate));
            }

            [Test]
            public void RelatedAnime() {
                Assert.That(this.SteinsGateDetailsPage.Adapation, Is.EqualTo(SteinsGate.Adapatation));
                Assert.That(this.SteinsGateDetailsPage.AlternativeSetting, Is.EqualTo(SteinsGate.AlternativeSetting));
            }
        }
    }
}
