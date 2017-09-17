using System;
using AnimeExporter.Controllers;
using AnimeExporter.Models;
using AnimeExporterTests.TestUtility;
using NUnit.Framework;

namespace AnimeExporterTests.test.Controllers {
    
    /// <summary>
    /// Contains NUnit tests for <see cref="VideoController"/>
    /// </summary>
    public class VideoControllerTest {

        // Create the controllers
        public static VideoController FMABrotherhoodVideoController =
            CreateData.FMABrotherhoodVideo();
        
        public static VideoController KimiNiTodokeVideoController =
            CreateData.KimiNiTodokeVideo();
        
        public static VideoController SteinsGateVideoController =
            CreateData.SteinsGateVideo();

        // Create the models
        public static VideoModel FMABrotherhoodModel =
            (VideoModel) FMABrotherhoodVideoController.TryScrape();
        
        public static VideoModel KimiNiTodokeVideoModel =
            (VideoModel) KimiNiTodokeVideoController.TryScrape();
        
        public static VideoModel SteinsGateVideoModel =
            (VideoModel) SteinsGateVideoController.TryScrape();

        [TestFixture]
        public class TestScraping {

            [Test]
            public void PromoVideo() {
                Assert.That(FMABrotherhoodModel.PromoVideo.Value, Is.EqualTo(TestConstants.FMABrotherhood.PromoVideo));
                Assert.That(KimiNiTodokeVideoModel.PromoVideo.Value, Is.EqualTo(TestConstants.KimiNiTodoke.PromoVideo));
                Assert.That(SteinsGateVideoModel.PromoVideo.Value, Is.EqualTo(TestConstants.SteinsGate.PromoVideo));
            }

            [Test]
            public void IsOnCrunchyRoll() {
                Assert.That(FMABrotherhoodModel.IsOnCrunchyRoll.Value, Is.EqualTo(TestConstants.FMABrotherhood.IsOnCrunchyRoll.ToString()));
                Assert.That(KimiNiTodokeVideoModel.IsOnCrunchyRoll.Value, Is.EqualTo(TestConstants.KimiNiTodoke.IsOnCrunchyRoll.ToString()));
                Assert.That(SteinsGateVideoModel.IsOnCrunchyRoll.Value, Is.EqualTo(TestConstants.SteinsGate.IsOnCrunchyRoll.ToString()));
            }

            [Test]
            public void IsOnHulu() {
                Assert.That(FMABrotherhoodModel.IsOnHulu.Value, Is.EqualTo(TestConstants.FMABrotherhood.IsOnHulu.ToString()));
                Assert.That(KimiNiTodokeVideoModel.IsOnHulu.Value, Is.EqualTo(TestConstants.KimiNiTodoke.IsOnHulu.ToString()));
                Assert.That(SteinsGateVideoModel.IsOnHulu.Value, Is.EqualTo(TestConstants.SteinsGate.IsOnHulu.ToString()));
            }

            [Test]
            public void HasStreaming() {
                Assert.That(FMABrotherhoodModel.HasStreaming.Value, Is.EqualTo(TestConstants.FMABrotherhood.HasStreaming.ToString()));
                Assert.That(KimiNiTodokeVideoModel.HasStreaming.Value, Is.EqualTo(TestConstants.KimiNiTodoke.HasStreaming.ToString()));
                Assert.That(SteinsGateVideoModel.HasStreaming.Value, Is.EqualTo(TestConstants.SteinsGate.HasStreaming.ToString()));
            }
        }
    }
}
