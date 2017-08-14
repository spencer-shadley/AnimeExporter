using System.Collections.Generic;
using System.Linq;

using AnimeExporter;
using NUnit.Framework;

namespace AnimeExporterTests.test {
    
    public class AnimesTest {
        private const string Title = "Kimi no na wa.";
        private const string Url = "https://myanimelist.net/anime/32281/Kimi_no_Na_wa";
        
        public Animes Animes;

        public AnimesTest() {
            this.Animes = new Animes();
        }

        [TestFixture]
        public class AddAnime {
            private const int StressAddNum = 1000;
            
            private AnimesTest _test;

            private List<Anime> AnimesList => this._test.Animes.ToList();

            [SetUp] public void Setup()       { this._test = new AnimesTest(); }
            [TearDown] public void Teardown() { this._test = null; }
            
            [Test]
            public void AddEmptyAnime() {
                this.VerifyAndAdd(new Anime());
            }

            [Test]
            public void AddSingleAnime() {
                this.VerifyAndAdd(CreateAnime());
            }

            [Test]
            public void AddManyAnime() {
                for (int i = 0; i < StressAddNum; ++i) {
                    this.VerifyAndAdd(CreateAnime(Url + i), i);
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
                var animesToAdd = new Animes {CreateAnime()};
                this._test.Animes.Add(animesToAdd);
                Assert.That(this.AnimesList, Has.Count.EqualTo(1));
            }

            [Test]
            public void AddManyAnimes() {
                var animesToAdd = new Animes();

                for (int i = 0; i < StressAddNum; ++i) {
                    animesToAdd.Add(CreateAnime(Url + i));
                }
                this._test.Animes.Add(animesToAdd);
                Assert.That(this.AnimesList, Has.Count.EqualTo(StressAddNum));
            }

            private void VerifyAndAdd(Anime anime, int initialSize = 0) {
                Assert.That(this.AnimesList, Has.Count.EqualTo(initialSize));
                
                this._test.Animes.Add(anime);
                Assert.That(this.AnimesList, Has.Count.EqualTo(initialSize+1));                
                Assert.AreEqual(this.AnimesList[initialSize], anime);
            }
        }

        private static Anime CreateAnime(string url = null) {
            return new Anime {
                Title = Title,
                Url = url ?? Url
            };
        }
    }
}
