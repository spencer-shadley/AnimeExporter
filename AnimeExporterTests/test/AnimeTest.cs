using AnimeExporter;
using NUnit.Framework;

using static AnimeExporterTests.utility.TestConstants;

namespace AnimeExporterTests.test {
    
    /// <summary>
    /// Contains NUnit tests for <see cref="Anime"/> and <see cref="Attribute"/>
    /// </summary>
    public class AnimeTest {
        
        public Anime Anime;

        public AnimeTest() {
            this.Anime = new Anime();
        }

        [TestFixture]
        public class TestAttributes {
            
            private AnimeTest _test;

            private Anime Anime {
                get => this._test.Anime;
                set => this._test.Anime = value;
            }

            [SetUp] public void Setup()       { this._test = new AnimeTest(); }
            [TearDown] public void Teardown() { this._test = null; }
            
            [Test]
            public void HasATitle() {
                Assert.That(this.Anime.AllAttributes, Contains.Item(new Attribute(DefaultAttributes.Title)));
                Assert.That(this.Anime.Title.Name, Is.EqualTo(DefaultAttributes.Title));
                Assert.That(this.Anime.Title.Value, Is.EqualTo(DefaultAttributes.Title));
                Assert.That(this.Anime.Title, Is.EqualTo(new Attribute(DefaultAttributes.Title)));
            }

            [Test]
            public void HasCorrectTitle() {
                this.Anime = new Anime {
                    Title = KimiNoNaWa.Title
                };
                Assert.That(this.Anime.Title.Value, Is.EqualTo(KimiNoNaWa.Title));
            }
            
            [Test]
            public void HasAUrl() {
                Assert.That(this.Anime.AllAttributes, Contains.Item(new Attribute(DefaultAttributes.Url)));
                Assert.That(this.Anime.Url.Name, Is.EqualTo(DefaultAttributes.Url));
                Assert.That(this.Anime.Url.Value, Is.EqualTo(DefaultAttributes.Url));
                Assert.That(this.Anime.Url, Is.EqualTo(new Attribute(DefaultAttributes.Url)));
            }

            [Test]
            public void HasCorrectUrl() {
                this.Anime = new Anime {
                    Url = KimiNoNaWa.Url
                };
                Assert.That(this.Anime.Url.Value, Is.EqualTo(KimiNoNaWa.Url));
            }

            [Test]
            public void HasEveryAttribute() {
                Assert.That(this.Anime.AllAttributes, Has.Count.EqualTo(31));
            }
        }
    }
}
