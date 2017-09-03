using AnimeExporter.Models;
using AnimeExporterTests.TestUtility;
using NUnit.Framework;

namespace AnimeExporterTests.test.Models {
    
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
                Assert.That(this.Anime.AllAttributes, Contains.Item(new Attribute(TestConstants.DefaultAttributes.Title)));
                Assert.That(this.Anime.Title.Name, Is.EqualTo(TestConstants.DefaultAttributes.Title));
                Assert.That(this.Anime.Title.Value, Is.EqualTo(TestConstants.DefaultAttributes.Title));
                Assert.That(this.Anime.Title, Is.EqualTo(new Attribute(TestConstants.DefaultAttributes.Title)));
            }

            [Test]
            public void HasCorrectTitle() {
                this.Anime = new Anime {
                    Title = TestConstants.KimiNoNaWa.Title
                };
                Assert.That(this.Anime.Title.Value, Is.EqualTo(TestConstants.KimiNoNaWa.Title));
            }
            
            [Test]
            public void HasAUrl() {
                Assert.That(this.Anime.AllAttributes, Contains.Item(new Attribute(TestConstants.DefaultAttributes.Url)));
                Assert.That(this.Anime.Url.Name, Is.EqualTo(TestConstants.DefaultAttributes.Url));
                Assert.That(this.Anime.Url.Value, Is.EqualTo(TestConstants.DefaultAttributes.Url));
                Assert.That(this.Anime.Url, Is.EqualTo(new Attribute(TestConstants.DefaultAttributes.Url)));
            }

            [Test]
            public void HasCorrectUrl() {
                this.Anime = new Anime {
                    Url = TestConstants.KimiNoNaWa.Url
                };
                Assert.That(this.Anime.Url.Value, Is.EqualTo(TestConstants.KimiNoNaWa.Url));
            }

            [Test]
            public void HasEveryAttribute() {
                Assert.That(this.Anime.AllAttributes, Has.Count.EqualTo(31));
            }
        }
    }
}
