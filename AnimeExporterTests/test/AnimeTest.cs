using System.Collections.Generic;
using AnimeExporter;
using NUnit.Framework;

namespace AnimeExporterTests.test {
    
    public class AnimeTest {
        private const string Title = "Kimi no na wa.";
        private const string Url = "https://myanimelist.net/anime/32281/Kimi_no_Na_wa";
        
        public Anime Anime;

        public AnimeTest() {
            this.Anime = new Anime();
        }

        [TestFixture]
        public class TestAllAttributes {
            
            private AnimeTest _test;

            private Anime Anime => this._test.Anime;

            [SetUp] public void Setup()       { this._test = new AnimeTest(); }
            [TearDown] public void Teardown() { this._test = null; }
            
            [Test]
            public void IncludesTitle() {
                Assert.AreEqual(this.Anime.Title.Name, "Title");
                Assert.AreEqual(this.Anime.Title.Value, "Title");
//                Assert.AreEqual(this.Anime.Title, new Attribute("Title"));
            }

            /*[Test]
            public void AddSingleAnime() {
                this.VerifyAndAdd(CreateBasicAnime());
            }

            [Test]
            public void AddManyAnime() {
                for (int i = 0; i < StressAddNum; ++i) {
                    this.VerifyAndAdd(CreateBasicAnime(Url + i), i);
                }
                Assert.That(this.AnimesList, Has.Count.EqualTo(StressAddNum));
            }

            [Test]
            public void AddEmptyAnimes() {
                var animesToAdd = new Animes();
                
                this._test.Animes.Add(animesToAdd);
                Assert.That(this.AnimesList, Is.Empty);
            }

            [Test]
            public void AddOneAnimes() {
                var animesToAdd = new Animes {CreateBasicAnime()};
                this._test.Animes.Add(animesToAdd);
                Assert.That(this.AnimesList, Has.Count.EqualTo(1));
            }

            [Test]
            public void AddManyAnimes() {
                var animesToAdd = new Animes();

                for (int i = 0; i < StressAddNum; ++i) {
                    animesToAdd.Add(CreateBasicAnime(Url + i));
                }
                this._test.Animes.Add(animesToAdd);
                Assert.That(this.AnimesList, Has.Count.EqualTo(StressAddNum));
            }

            private void VerifyAndAdd(Anime anime, int initialSize = 0) {
                Assert.That(this.AnimesList, Has.Count.EqualTo(initialSize));
                
                this._test.Animes.Add(anime);
                Assert.That(this.AnimesList, Has.Count.EqualTo(initialSize+1));                
                Assert.AreEqual(this.AnimesList[initialSize], anime);
            }*/
        }

        private static Anime CreateBasicAnime(string url = null) {
            return new Anime {
                Title = Title,
                Url = url ?? Url
            };
        }
    }
}
