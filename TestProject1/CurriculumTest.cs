using Model;
using System.Collections.Generic;
using Xunit;

namespace TestProject1
{
    public class CurriculumTest
    {
        [Fact]
        public void Curriculumをテスト()
        {
            var curriculum = new Curriculum(10, 10);
            curriculum[0, 0] = new CurriculumCell("国語", new List<string> { "坂本", "松井", });

            Assert.Equal("国語", curriculum[0, 0].Lesson);
            Assert.Equal("坂本", curriculum[0, 0].Teachers[0]);
            Assert.Equal("松井", curriculum[0, 0].Teachers[1]);
        }
    }
}