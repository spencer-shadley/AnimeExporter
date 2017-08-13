using NUnit.Framework;

namespace AnimeExporter.test {
    
    [TestFixture]
    public class AnimesTest {
        private bool? _yes;
        private bool? _no;
        
        [SetUp]
        public void Init() {
            Assert.IsNull(this._yes);
            Assert.IsNull(this._no);

            this._yes = true;
            this._no = false;
        }

        [TearDown]
        public void Cleanup() {
            Assert.IsNotNull(this._yes);
            Assert.IsNotNull(this._no);
            
            this._yes = null;
            this._no = null;
        }
        
        [Test]
        public void PassTest() {
            Assert.IsTrue(this._yes);
            Assert.IsFalse(this._no);
        }

        [Test]
        public void FailTest() {
            Assert.IsNull(this._yes);
            Assert.IsNull(this._no);
        }
    }
}