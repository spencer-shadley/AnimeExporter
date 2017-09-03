using AnimeExporter.Controllers;
using AnimeExporterTests.TestUtility;
using NUnit.Framework;

namespace AnimeExporterTests.test.Controllers {
    
    /// <summary>
    /// Contains NUnit tests for <see cref="AnimeDetailsPage"/>
    /// </summary>
    public class AnimeDetailsPageTest {

        public static AnimeDetailsPage KimiDetailsPage =
            CreateData.KimiNoNaWaDetailsPage();
        public static AnimeDetailsPage SteinsGateDetailsPage =
            CreateData.SteinsGateDetailsPage();
        public static AnimeDetailsPage OwarimonogatariSecondSeasonDetailsPage =
            CreateData.OwarimongatariSecondSeasonDetailsPage();

        [TestFixture]
        public class TestScraping {
            [Test]
            public void Title() {
                Assert.That(KimiDetailsPage.Title, Is.EqualTo(TestConstants.KimiNoNaWa.Title));
            }

            [Test]
            public void Rank() {
                int rank = int.Parse(KimiDetailsPage.Rank); 
                
                // assuming Kimi no Na Wa will always be at ranked at least 1,000
                Assert.That(rank, Is.LessThan(1000));  
                Assert.That(rank, Is.GreaterThan(0));
            }

            [Test]
            public void MediaType() {
                Assert.That(KimiDetailsPage.MediaType, Is.EqualTo(TestConstants.KimiNoNaWa.Type));
            }

            [Test]
            public void Image() {
                Assert.That(KimiDetailsPage.ImageUrl, Is.EqualTo(TestConstants.KimiNoNaWa.ImageUrl));
            }

            [Test]
            public void Score() {
                double score = double.Parse(KimiDetailsPage.Score);
                Assert.That(score, Is.LessThan(9.9));
                Assert.That(score, Is.GreaterThan(8.5));
            }

            [Test]
            public void Background() {
                StringAssert.Contains(TestConstants.SteinsGate.PartialBackground, SteinsGateDetailsPage.Background);
            }

            [Test]
            public void NoBackground() {
                Assert.IsNull(KimiDetailsPage.Background);
            }

            [Test]
            public void EnglishTitle() {
                Assert.That(KimiDetailsPage.EnglishTitle, Is.EqualTo(TestConstants.KimiNoNaWa.EnglishTitle));
            }

            [Test]
            public void Genres() {
                Assert.That(KimiDetailsPage.Genres, Is.EqualTo(TestConstants.KimiNoNaWa.Genres));
            }

            [Test]
            public void AirStartDate() {
                Assert.That(SteinsGateDetailsPage.AirStartDate, Is.EqualTo(TestConstants.SteinsGate.AirStartDate));
            }

            [Test]
            public void AirFinishDate() {
                Assert.That(SteinsGateDetailsPage.AirFinishDate, Is.EqualTo(TestConstants.SteinsGate.AirFinishDate));
            }
            
            [Test]
            public void RelatedAnime() {
                Assert.That(SteinsGateDetailsPage.Adapation, Is.EqualTo(TestConstants.SteinsGate.Adapatation));
                Assert.That(SteinsGateDetailsPage.AlternativeSetting, Is.EqualTo(TestConstants.SteinsGate.AlternativeSetting));
                Assert.That(OwarimonogatariSecondSeasonDetailsPage.Prequel, Is.EqualTo(TestConstants.OwarimongatariSecondSeason.Prequel));
                Assert.That(OwarimonogatariSecondSeasonDetailsPage.Sequel, Is.EqualTo(TestConstants.OwarimongatariSecondSeason.Sequel));
            }
        }
    }
}
