using AnimeExporter.Controllers;
using AnimeExporter.Models;
using AnimeExporterTests.TestUtility;
using NUnit.Framework;

namespace AnimeExporterTests.test.Controllers {
    
    /// <summary>
    /// Contains NUnit tests for <see cref="StatsController"/>
    /// </summary>
    public class StatsControllerTest {

        // Create the controllers
        public static StatsController FMABrotherhoodCharactersController =
            CreateData.FMABrotherhoodStats();

        // Create the models
        public static StatsModel FMABrotherhoodModel =
            (StatsModel) FMABrotherhoodCharactersController.TryScrape();

        [TestFixture]
        public class TestScraping {

            [Test]
            public void Watching() {
                Assert.That(int.Parse(FMABrotherhoodModel.Watching.Value), Is.LessThan   (500000));
                Assert.That(int.Parse(FMABrotherhoodModel.Watching.Value), Is.GreaterThan(50000));
            }

            [Test]
            public void Completed() {
                Assert.That(int.Parse(FMABrotherhoodModel.Completed.Value), Is.LessThan   (5000000));
                Assert.That(int.Parse(FMABrotherhoodModel.Completed.Value), Is.GreaterThan(722000));
            }

            [Test]
            public void Total() {
                Assert.That(int.Parse(FMABrotherhoodModel.Total.Value), Is.LessThan   (1000000));
                Assert.That(int.Parse(FMABrotherhoodModel.Total.Value), Is.GreaterThan(993989));
                Assert.That(int.Parse(FMABrotherhoodModel.Total.Value), Is.EqualTo(CalculateTotal(FMABrotherhoodModel)));
            }

            [Test]
            public void NumberScoreTen() {
                Assert.That(int.Parse(FMABrotherhoodModel.NumberScoreTen.Value), Is.LessThan   (10000000));
                Assert.That(int.Parse(FMABrotherhoodModel.NumberScoreTen.Value), Is.GreaterThan(300000));
            }

            [Test]
            public void NumberScoreThree() {
                Assert.That(int.Parse(FMABrotherhoodModel.NumberScoreThree.Value), Is.LessThan   (10000));
                Assert.That(int.Parse(FMABrotherhoodModel.NumberScoreThree.Value), Is.GreaterThan(500));
            }

            [Test]
            public void NumberScoreOne() {
                Assert.That(int.Parse(FMABrotherhoodModel.NumberScoreOne.Value), Is.LessThan   (10000));
                Assert.That(int.Parse(FMABrotherhoodModel.NumberScoreOne.Value), Is.GreaterThan(1000));
            }

            [Test]
            public void PercentScoreTen() {
                Assert.That(double.Parse(FMABrotherhoodModel.PercentScoreTen.Value), Is.LessThan   (65));
                Assert.That(double.Parse(FMABrotherhoodModel.PercentScoreTen.Value), Is.GreaterThan(45));
            }

            private static int CalculateTotal(StatsModel model) {
                return int.Parse(model.Watching.Value) +
                       int.Parse(model.Completed.Value) +
                       int.Parse(model.OnHold.Value) +
                       int.Parse(model.Dropped.Value) +
                       int.Parse(model.PlanToWatch.Value);
            }
        }
    }
}
