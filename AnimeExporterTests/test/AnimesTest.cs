using System.Collections.Generic;
using System.Linq;

using AnimeExporter;
using NUnit.Framework;

using Attribute = AnimeExporter.Attribute;

namespace AnimeExporterTests.test {
    
    public class AnimesTest {
        private const string Title = "Kimi no na wa.";
        private const string Url = "https://myanimelist.net/anime/32281/Kimi_no_Na_wa";
        
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

            private List<Anime> AnimesList => this._test.Animes.ToList();

            [SetUp] public void Setup()       { this._test = new AnimesTest(); }
            [TearDown] public void Teardown() { this._test = null; }
            
            [Test]
            public void AddEmpty() {
                this.VerifyAdd(new Anime());
            }

            [Test]
            public void AddSingleAnime() {
                this.VerifyAdd(CreateAnime());
            }

            [Test]
            public void AddManyAnime() {
                for (int i = 0; i < 1000; ++i) {
                    Anime anime = CreateAnime();
                    anime.Url = new Attribute(Url + i);
                    this.VerifyAdd(anime, i);
                }
            }

            [Test]
            public void AddAnimes() {
                
            }

            private void VerifyAdd(Anime anime, int initialSize = 0) {
                Assert.That(this.AnimesList, Has.Count.EqualTo(initialSize));
                
                this._test.Animes.Add(anime);
                Assert.That(this.AnimesList, Has.Count.EqualTo(initialSize+1));                
                Assert.AreEqual(this.AnimesList[initialSize], anime);
            }
        }

        private static Anime CreateAnime() {
            return new Anime {
                Title = new Attribute(Title),
                Url = new Attribute(Url)
            };
        }
    }
}
