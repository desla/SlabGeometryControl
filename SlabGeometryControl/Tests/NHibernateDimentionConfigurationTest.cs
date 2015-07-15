using Alvasoft.DimentionConfiguration.NHibernateImpl;
using NUnit.Framework;

namespace Alvasoft.Tests
{
    [TestFixture]
    public class NHibernateDimentionConfigurationTest
    {
        [Test]
        public void InitializeConfigurationTest()
        {
            var configuration = new NHibernateDimentionConfigurationImpl();
            configuration.Initialize();

            var width = configuration.GetDimentionInfoByName("width");
            Assert.IsNotNull(width);
            Assert.AreEqual(width.GetName(), "width");
        }
    }
}
