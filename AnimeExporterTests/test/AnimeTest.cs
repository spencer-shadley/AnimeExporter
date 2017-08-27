using AnimeExporter;
using NUnit.Framework;

using static AnimeExporterTests.data.TestConstants;

namespace AnimeExporterTests.test {
    
    public class AnimeTest {
        
        public Anime Anime;

        public AnimeTest() {
            this.Anime = new Anime();
        }

        [TestFixture]
        public class TestAllAttributes {
            
            private AnimeTest _test;

            private Anime Anime {
                get => this._test.Anime;
                set => this._test.Anime = value;
            }

            [SetUp] public void Setup()       { this._test = new AnimeTest(); }
            [TearDown] public void Teardown() { this._test = null; }
            
            [Test]
            public void HasATitle() {
                Assert.AreEqual(this.Anime.Title.Name, DefaultAttributes.Title);
                Assert.AreEqual(this.Anime.Title.Value, DefaultAttributes.Title);
                Assert.AreEqual(this.Anime.Title, new Attribute(DefaultAttributes.Title));
            }

            [Test]
            public void HasCorrectTitle() {
                this.Anime = new Anime {
//                    Title = 
                };
            }
            
            [Test]
            public void HasAUrl() {
                Assert.AreEqual(this.Anime.Url.Name, DefaultAttributes.Url);
                Assert.AreEqual(this.Anime.Url.Value, DefaultAttributes.Url);
                Assert.AreEqual(this.Anime.Url, new Attribute(DefaultAttributes.Url));
            }

            [Test]
            public void HasEveryAttribute() {
                Assert.That(this.Anime.AllAttributes, Has.Count.EqualTo(31));
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
    }
}
