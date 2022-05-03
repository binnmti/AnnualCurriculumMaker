using Model;
using System.Collections.Generic;
using Xunit;

namespace TestProject1
{
    public class CurriculumTest
    {
        [Fact]
        public void Curriculum���e�X�g()
        {
            var curriculum = new Curriculum(10, 10);
            curriculum[0, 0] = new CurriculumCell("����", new List<string> { "��{", "����", });

            Assert.Equal("����", curriculum[0, 0].Lesson);
            Assert.Equal("��{", curriculum[0, 0].Teachers[0]);
            Assert.Equal("����", curriculum[0, 0].Teachers[1]);
        }
    }
}