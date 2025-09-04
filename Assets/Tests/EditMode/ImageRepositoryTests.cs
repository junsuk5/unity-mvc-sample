using System.Threading.Tasks;
using Data.DataSource;
using Data.Repository;
using NUnit.Framework;

namespace Tests.EditMode
{
    public class ImageRepositoryTests
    {
        [TestFixture]
        [TestOf(typeof(ImageRepository))]
        public class ImageRepositoryTest
        {
            private IImageRepository _imageRepository;
            private IImageDataSource _mockImageDataSource;

            [SetUp]
            public void Setup()
            {
                _mockImageDataSource = new MockImageDataSource();
                _imageRepository = new ImageRepository(imageDataSource: _mockImageDataSource);
            }

            [Test]
            public async Task ImageRepository_동작_확인()
            {
                var images = await _imageRepository.GetImageAsync("apple");
                Assert.That(images.Count, Is.GreaterThan(0));
            }
        }
    }
}