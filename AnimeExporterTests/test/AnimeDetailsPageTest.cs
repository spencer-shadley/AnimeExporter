using AnimeExporter;
using NUnit.Framework;

using static AnimeExporterTests.data.TestConstants;

namespace AnimeExporterTests.test {
    
    /// <summary>
    /// Contains NUnit tests for <see cref="AnimeDetailsPage"/>
    /// </summary>
    public class AnimeDetailsPageTest {
        
        public Anime Anime;

        public AnimeDetailsPageTest() {
            this.Anime = new Anime();
        }

        [TestFixture]
        public class TesetParsing {
            
            private AnimeDetailsPage _test;

            private AnimeDetailsPage Details {
                get => this._test;
                set => this._test = value;
            }

            [SetUp] public void Setup()       { this._test = new AnimeDetailsPage(); }
            [TearDown] public void Teardown() { this._test = null; }
            
            [Test]
            public void HasATitle() {
                Assert.That(this.Anime.AllAttributes, Contains.Item(new Attribute(DefaultAttributes.Title)));
                Assert.AreEqual(this.Anime.Title.Name, DefaultAttributes.Title);
                Assert.AreEqual(this.Anime.Title.Value, DefaultAttributes.Title);
                Assert.AreEqual(this.Anime.Title, new Attribute(DefaultAttributes.Title));
            }

            [Test]
            public void HasCorrectTitle() {
                this.Anime = new Anime {
                    Title = KimiNoNaWa.Title
                };
                Assert.AreEqual(this.Anime.Title.Value, KimiNoNaWa.Title);
            }
            
            [Test]
            public void HasAUrl() {
                Assert.That(this.Anime.AllAttributes, Contains.Item(new Attribute(DefaultAttributes.Url)));
                Assert.AreEqual(this.Anime.Url.Name, DefaultAttributes.Url);
                Assert.AreEqual(this.Anime.Url.Value, DefaultAttributes.Url);
                Assert.AreEqual(this.Anime.Url, new Attribute(DefaultAttributes.Url));
            }

            [Test]
            public void HasCorrectUrl() {
                this.Anime = new Anime {
                    Url = KimiNoNaWa.Url
                };
                Assert.AreEqual(this.Anime.Url.Value, KimiNoNaWa.Url);
            }

            [Test]
            public void HasEveryAttribute() {
                Assert.That(this.Anime.AllAttributes, Has.Count.EqualTo(31));
            }
        }
    }
}
