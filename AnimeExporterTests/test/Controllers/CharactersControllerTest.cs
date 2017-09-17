using AnimeExporter.Controllers;
using AnimeExporter.Models;
using AnimeExporterTests.TestUtility;
using NUnit.Framework;

namespace AnimeExporterTests.test.Controllers {
    
    /// <summary>
    /// Contains NUnit tests for <see cref="CharactersController"/>
    /// </summary>
    public class CharactersControllerTest {

        // Create the controllers
        public static CharactersController FMABrotherhoodCharactersController =
            CreateData.FMABrotherhoodCharacters();
        
        public static CharactersController KimiNiTodokeController =
            CreateData.KimiNiTodokeCharacters();

        // Create the models
        public static CharactersModel FMABrotherhoodModel =
            (CharactersModel) FMABrotherhoodCharactersController.TryScrape();
        
        public static CharactersModel KimiNiTodokeModel =
            (CharactersModel) KimiNiTodokeController.TryScrape();

        [TestFixture]
        public class TestScraping {

            [Test]
            public void English() {
                Assert.That(FMABrotherhoodModel.IsInEnglish.Value, Is.EqualTo(TestConstants.FMABrotherhood.IsInEnglish.ToString()));
                Assert.That(KimiNiTodokeModel.IsInEnglish.Value, Is.EqualTo(TestConstants.KimiNiTodoke.IsInEnglish.ToString()));
            }

            [Test]
            public void Japanese() {
                Assert.That(FMABrotherhoodModel.IsInJapanese.Value, Is.EqualTo(TestConstants.FMABrotherhood.IsInJapanese.ToString()));
                Assert.That(KimiNiTodokeModel.IsInJapanese.Value, Is.EqualTo(TestConstants.KimiNiTodoke.IsInJapanese.ToString()));
            }

            [Test]
            public void Spanish() {
                Assert.That(FMABrotherhoodModel.IsInSpanish.Value, Is.EqualTo(TestConstants.FMABrotherhood.IsInSpanish.ToString()));
                Assert.That(KimiNiTodokeModel.IsInSpanish.Value, Is.EqualTo(TestConstants.KimiNiTodoke.IsInSpanish.ToString()));
            }

            [Test]
            public void Producer() {
                Assert.That(FMABrotherhoodModel.Producer.Value, Is.EqualTo(TestConstants.FMABrotherhood.Producer));
                Assert.That(KimiNiTodokeModel.Producer.Value, Is.EqualTo(TestConstants.KimiNiTodoke.Producer));
            }

            [Test]
            public void Director() {
                Assert.That(FMABrotherhoodModel.Director.Value, Is.EqualTo(TestConstants.FMABrotherhood.Director));
                Assert.That(KimiNiTodokeModel.Director.Value, Is.EqualTo(TestConstants.KimiNiTodoke.Director));
            }

            [Test]
            public void SoundDirector() {
                Assert.That(FMABrotherhoodModel.SoundDirector.Value, Is.EqualTo(TestConstants.FMABrotherhood.SoundDirector));
                Assert.That(KimiNiTodokeModel.SoundDirector.Value, Is.EqualTo(TestConstants.KimiNiTodoke.SoundDirector));
            }

            [Test]
            public void ExecutiveProducer() {
                Assert.That(FMABrotherhoodModel.ExecutiveProducer.Value, Is.EqualTo(TestConstants.FMABrotherhood.ExecutiveProducer));
                Assert.That(KimiNiTodokeModel.ExecutiveProducer.Value, Is.EqualTo(TestConstants.KimiNiTodoke.ExecutiveProducer));
            }
        }
    }
}
