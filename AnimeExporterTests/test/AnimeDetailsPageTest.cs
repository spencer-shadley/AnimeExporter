using AnimeExporter;
using AnimeExporterTests.utility;
using NUnit.Framework;

using static AnimeExporterTests.utility.TestConstants;

namespace AnimeExporterTests.test {
    
    /// <summary>
    /// Contains NUnit tests for <see cref="AnimeDetailsPage"/>
    /// </summary>
    public class AnimeDetailsPageTest {
        
        public AnimeDetailsPage DetailsPage;

        public AnimeDetailsPageTest() {
            this.DetailsPage = CreateData.KimiNoNaWaDetailsPage();
        }

        [TestFixture]
        public class TestScraping {
            private AnimeDetailsPage DetailsPage { get; set; }

            [SetUp] public void Setup()       { this.DetailsPage = CreateData.KimiNoNaWaDetailsPage(); }
            [TearDown] public void Teardown() { this.DetailsPage = null; }

            [Test]
            public void Title() {
                Assert.That(this.DetailsPage.Title, Is.EqualTo(KimiNoNaWa.Title));
            }

            [Test]
            public void Rank() {
                int rank = int.Parse(this.DetailsPage.Rank); 
                
                // assuming Kimi no Na Wa will always be at ranked at least 1,000
                Assert.That(rank, Is.LessThan(1000));  
                Assert.That(rank, Is.GreaterThan(0));
            }

            [Test]
            public void MediaType() {
                Assert.That(this.DetailsPage.MediaType, Is.EqualTo(KimiNoNaWa.Type));
            }

            [Test]
            public void Image() {
                Assert.That(this.DetailsPage.ImageUrl, Is.EqualTo(KimiNoNaWa.ImageUrl));
            }

            [Test]
            public void Score() {
                double score = double.Parse(this.DetailsPage.Score);
                Assert.That(score, Is.LessThan(9.9));
                Assert.That(score, Is.GreaterThan(8.5));
            }

            [Test]
            public void Background() {
                this.DetailsPage = new AnimeDetailsPage(SteinsGate.Url);
                StringAssert.Contains(SteinsGate.PartialBackground, this.DetailsPage.Background);
            }

            [Test]
            public void NoBackground() {
                Assert.IsNull(this.DetailsPage.Background);
            }

            [Test]
            public void EnglishTitle() {
                Assert.That(this.DetailsPage.EnglishTitle, Is.EqualTo(KimiNoNaWa.EnglishTitle));
            }

            [Test]
            public void Genres() {
                Assert.That(this.DetailsPage.Genres, Is.EqualTo("Supernatural; Drama; Romance"));
            }

            [Test]
            public void AirStartDate() {
                
            }
        }
    }
}
