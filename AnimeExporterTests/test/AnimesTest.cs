using System.Collections.Generic;
using System.Linq;

using AnimeExporter;
using NUnit.Framework;

using Attribute = AnimeExporter.Attribute;

namespace AnimeExporterTests.test {
    
    public class AnimesTest {
        private const string Title = "Kimi no na wa."; 
        
        public Animes Animes;

        public AnimesTest() {
            this.Animes = new Animes();
        }

        // TODO: this should be a fixture
        [Test]
        public void TestConstruction() {
            // TODO
        }

        [TestFixture]
        public class AddAnime {

            private AnimesTest _test;

            [SetUp]
            public void Setup() {
                this._test = new AnimesTest();
            }

            [TearDown]
            public void Teardown() {
                this._test = null;
            }

            [Test]
            public void AddNull() {
                
            }
            
            [Test]
            public void AddEmpty() {
                
            }

            [Test]
            public void AddSingleAnime() {
                Anime animeToAdd = CreateAnime();
                this._test.Animes.Add(animeToAdd);

                List<Anime> animes = this._test.Animes.ToList();
            
                Assert.That(animes.ToList(), Has.Count.EqualTo(1));
            
                Anime addedAnime = animes[0];
                Assert.AreEqual(addedAnime, animeToAdd);
            }

            [Test]
            public void AddManyAnime() {
                
            }

            [Test]
            public void AddAnimes() {
                
            }
            
        }

        [Test]
        public void TestAddAnime() {
            
            // empty anime
            
            // anime with stuff
            
            // null

            Anime animeToAdd = CreateAnime();
            this.Animes.Add(animeToAdd);

            List<Anime> animes = this.Animes.ToList();
            
            Assert.That(animes.ToList(), Has.Count.EqualTo(1));
            
            Anime addedAnime = animes[0];
            Assert.AreEqual(addedAnime, animeToAdd);
        }
        
        [Test]
        public void TestAddAnimes() {
            const string title = "Kimi no na wa.";
            
            var animeToAdd = new Anime {Title = new Attribute(title)};
            this.Animes.Add(animeToAdd);

            List<Anime> animes = this.Animes.ToList();
            
            Assert.That(animes.ToList(), Has.Count.EqualTo(1));
            
            Anime addedAnime = animes[0];
            Assert.AreEqual(addedAnime, animeToAdd);
        }

        private static Anime CreateAnime() {
            return new Anime {Title = new Attribute(Title)};
        }
    }
}
