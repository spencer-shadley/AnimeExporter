using System.Collections.Generic;
using System.Linq;
using AnimeExporter.Models;
using AnimeExporterTests.TestUtility;
using NUnit.Framework;

namespace AnimeExporterTests.test.Models {
    
    /// <summary>
    /// Contains NUnit tests for <see cref="AnimesModel"/>
    /// </summary>
    public class AnimesModelTest {
        
        public AnimesModel AnimesModel;

        public AnimesModelTest() {
            this.AnimesModel = new AnimesModel();
        }

        [TestFixture]
        public class TestAddAnime {
            private const int StressAddNum = 1000;
            
            private AnimesModelTest _modelTest;

            private List<AnimeModel> AnimesList => this._modelTest.AnimesModel.ToList();

            [SetUp] public void Setup()       { this._modelTest = new AnimesModelTest(); }
            [TearDown] public void Teardown() { this._modelTest = null; }
            
            [Test]
            public void AddEmptyAnime() {
                this.VerifyAndAdd(new AnimeModel(string.Empty));
            }

            [Test]
            public void AddSingleAnime() {
                this.VerifyAndAdd(CreateData.KimiNoNaWa());
            }

            [Test]
            public void AddManyAnime() {
                for (int i = 0; i < StressAddNum; ++i) {
                    this.VerifyAndAdd(CreateData.KimiNoNaWa(TestConstants.KimiNoNaWa.Url + i), i);
                }
                Assert.That(this.AnimesList, Has.Count.EqualTo(StressAddNum));
            }

            [Test]
            public void AddEmptyAnimes() {
                var animesToAdd = new AnimesModel();
                
                this._modelTest.AnimesModel.Add(animesToAdd);
                Assert.That(this.AnimesList, Is.Empty);
            }

            [Test]
            public void AddOneAnimes() {
                var animesToAdd = new AnimesModel {CreateData.KimiNoNaWa()};
                this._modelTest.AnimesModel.Add(animesToAdd);
                Assert.That(this.AnimesList, Has.Count.EqualTo(1));
            }

            [Test]
            public void AddManyAnimes() {
                var animesToAdd = new AnimesModel();

                for (int i = 0; i < StressAddNum; ++i) {
                    animesToAdd.Add(CreateData.KimiNoNaWa(TestConstants.KimiNoNaWa.Url + i));
                }
                this._modelTest.AnimesModel.Add(animesToAdd);
                Assert.That(this.AnimesList, Has.Count.EqualTo(StressAddNum));
            }

            private void VerifyAndAdd(AnimeModel detailsModel, int initialSize = 0) {
                Assert.That(this.AnimesList, Has.Count.EqualTo(initialSize));
                
                this._modelTest.AnimesModel.Add(detailsModel);
                Assert.That(this.AnimesList, Has.Count.EqualTo(initialSize+1));                
                Assert.That(this.AnimesList[initialSize], Is.EqualTo(detailsModel));
            }
        }
    }
}
