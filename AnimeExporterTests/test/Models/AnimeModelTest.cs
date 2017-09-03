using AnimeExporter.Models;
using AnimeExporterTests.TestUtility;
using NUnit.Framework;

namespace AnimeExporterTests.test.Models {
    
    /// <summary>
    /// Contains NUnit tests for <see cref="DetailsModel"/> and <see cref="AttributeModel"/>
    /// </summary>
    public class AnimeModelTest {
        
        public DetailsModel DetailsModel;

        public AnimeModelTest() {
            this.DetailsModel = new DetailsModel();
        }

        [TestFixture]
        public class TestAttributes {
            
            private AnimeModelTest _modelTest;

            private DetailsModel DetailsModel {
                get => this._modelTest.DetailsModel;
                set => this._modelTest.DetailsModel = value;
            }

            [SetUp] public void Setup()       { this._modelTest = new AnimeModelTest(); }
            [TearDown] public void Teardown() { this._modelTest = null; }
            
            [Test]
            public void HasATitle() {
                Assert.That(this.DetailsModel.Attributes, Contains.Item(new AttributeModel(TestConstants.DefaultAttributes.Title)));
                Assert.That(this.DetailsModel.Title.Name, Is.EqualTo(TestConstants.DefaultAttributes.Title));
                Assert.That(this.DetailsModel.Title.Value, Is.EqualTo(TestConstants.DefaultAttributes.Title));
                Assert.That(this.DetailsModel.Title, Is.EqualTo(new AttributeModel(TestConstants.DefaultAttributes.Title)));
            }

            [Test]
            public void HasCorrectTitle() {
                this.DetailsModel = new DetailsModel {
                    Title = TestConstants.KimiNoNaWa.Title
                };
                Assert.That(this.DetailsModel.Title.Value, Is.EqualTo(TestConstants.KimiNoNaWa.Title));
            }
            
            [Test]
            public void HasAUrl() {
                Assert.That(this.DetailsModel.Attributes, Contains.Item(new AttributeModel(TestConstants.DefaultAttributes.Url)));
                Assert.That(this.DetailsModel.Url.Name, Is.EqualTo(TestConstants.DefaultAttributes.Url));
                Assert.That(this.DetailsModel.Url.Value, Is.EqualTo(TestConstants.DefaultAttributes.Url));
                Assert.That(this.DetailsModel.Url, Is.EqualTo(new AttributeModel(TestConstants.DefaultAttributes.Url)));
            }

            [Test]
            public void HasCorrectUrl() {
                this.DetailsModel = new DetailsModel {
                    Url = TestConstants.KimiNoNaWa.Url
                };
                Assert.That(this.DetailsModel.Url.Value, Is.EqualTo(TestConstants.KimiNoNaWa.Url));
            }

            [Test]
            public void HasEveryAttribute() {
                Assert.That(this.DetailsModel.Attributes, Has.Count.EqualTo(38));
            }
        }
    }
}
