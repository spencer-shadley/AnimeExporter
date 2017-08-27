using System.Collections.Generic;
using System.Linq;

using AnimeExporter;
using AnimeExporterTests.data;
using NUnit.Framework;

using static AnimeExporterTests.data.TestConstants;

namespace AnimeExporterTests.test {
    
    public class AnimesTest {
        
        public Animes Animes;

        public AnimesTest() {
            this.Animes = new Animes();
        }

        [TestFixture]
        public class TestAddAnime {
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
                this.VerifyAndAdd(Data.CreateKimiNoNaWa());
            }

            [Test]
            public void AddManyAnime() {
                for (int i = 0; i < StressAddNum; ++i) {
                    this.VerifyAndAdd(Data.CreateKimiNoNaWa(KimiNoNaWa.Url + i), i);
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
                var animesToAdd = new Animes {Data.CreateKimiNoNaWa()};
                this._test.Animes.Add(animesToAdd);
                Assert.That(this.AnimesList, Has.Count.EqualTo(1));
            }

            [Test]
            public void AddManyAnimes() {
                var animesToAdd = new Animes();

                for (int i = 0; i < StressAddNum; ++i) {
                    animesToAdd.Add(Data.CreateKimiNoNaWa(KimiNoNaWa.Url + i));
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
    }
}
