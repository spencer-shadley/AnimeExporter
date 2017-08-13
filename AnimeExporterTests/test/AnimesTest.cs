using System.Collections.Generic;
using System.Linq;

using AnimeExporter;
using NUnit.Framework;

namespace AnimeExporterTests.test {
    
    [TestFixture]
    public class AnimesTest {
        private Animes _animes;
        
        [SetUp]
        public void Init() {
            this._animes = new Animes();
        }

        [TearDown]
        public void Cleanup() {
            this._animes = null;
        }

        [Test]
        public void TestConstruction() {
            // TODO
        }

        [Test]
        public void TestAdd() {
            const string title = "Kimi no na wa.";
            
            var animeToAdd = new Anime {Title = new Attribute(title)};
            this._animes.Add(animeToAdd);

            List<Anime> animes = this._animes.ToList();
            
            Assert.That(animes.ToList(), Has.Count.EqualTo(1));
            
            Anime addedAnime = animes[0];
            Assert.AreEqual(addedAnime, animeToAdd);
        }
    }
}
