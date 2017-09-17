using AnimeExporter.Controllers;
using AnimeExporterTests.TestUtility;
using NUnit.Framework;

namespace AnimeExporterTests.test.Controllers {
    
    /// <summary>
    /// Contains NUnit tests for <see cref="DetailsController"/>
    /// </summary>
    public class DetailsControllerTest {

        public static DetailsController KimiDetailsController =
            CreateData.KimiNoNaWaDetails();
        
        public static DetailsController SteinsGateDetailsController =
            CreateData.SteinsGateDetails();
        
        public static DetailsController OwarimonogatariSecondSeasonDetailsController =
            CreateData.OwariMongatariSecondSeasonDetails();

        [TestFixture]
        public class TestScraping {
            [Test]
            public void Title() {
                Assert.That(KimiDetailsController.Title, Is.EqualTo(TestConstants.KimiNoNaWa.Title));
            }

            [Test]
            public void Rank() {
                int rank = int.Parse(KimiDetailsController.Rank); 
                
                // assuming Kimi no Na Wa will always be at ranked at least 1,000
                Assert.That(rank, Is.LessThan(1000));  
                Assert.That(rank, Is.GreaterThan(0));
            }

            [Test]
            public void MediaType() {
                Assert.That(KimiDetailsController.MediaType, Is.EqualTo(TestConstants.KimiNoNaWa.Type));
            }

            [Test]
            public void Image() {
                Assert.That(KimiDetailsController.ImageUrl, Is.EqualTo(TestConstants.KimiNoNaWa.ImageUrl));
            }

            [Test]
            public void Score() {
                double score = double.Parse(KimiDetailsController.Score);
                Assert.That(score, Is.LessThan(9.9));
                Assert.That(score, Is.GreaterThan(8.5));
            }

            [Test]
            public void Background() {
                StringAssert.Contains(TestConstants.SteinsGate.PartialBackground, SteinsGateDetailsController.Background);
            }

            [Test]
            public void NoBackground() {
                Assert.IsNull(KimiDetailsController.Background);
            }

            [Test]
            public void EnglishTitle() {
                Assert.That(KimiDetailsController.EnglishTitle, Is.EqualTo(TestConstants.KimiNoNaWa.EnglishTitle));
            }

            [Test]
            public void Genres() {
                Assert.That(KimiDetailsController.Genres, Is.EqualTo(TestConstants.KimiNoNaWa.Genres));
            }

            [Test]
            public void AirStartDate() {
                Assert.That(SteinsGateDetailsController.AirStartDate, Is.EqualTo(TestConstants.SteinsGate.AirStartDate));
            }

            [Test]
            public void AirFinishDate() {
                Assert.That(SteinsGateDetailsController.AirFinishDate, Is.EqualTo(TestConstants.SteinsGate.AirFinishDate));
            }
            
            [Test]
            public void RelatedAnime() {
                Assert.That(SteinsGateDetailsController.Adapation, Is.EqualTo(TestConstants.SteinsGate.Adapatation));
                Assert.That(SteinsGateDetailsController.AlternativeSetting, Is.EqualTo(TestConstants.SteinsGate.AlternativeSetting));
                Assert.That(OwarimonogatariSecondSeasonDetailsController.Prequel, Is.EqualTo(TestConstants.OwarimongatariSecondSeason.Prequel));
                Assert.That(OwarimonogatariSecondSeasonDetailsController.Sequel, Is.EqualTo(TestConstants.OwarimongatariSecondSeason.Sequel));
            }
        }
    }
}
