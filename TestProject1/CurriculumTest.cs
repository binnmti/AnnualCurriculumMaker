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
            var lesson = new Lesson("����", "1Q", "���j", "�P�N", "2��");
            curriculum[0, 0] = new CurriculumCell(lesson, new List<string> { "��{", "����", });

            Assert.Equal("����", curriculum[0, 0].Lesson.Name);
            Assert.Equal("��{", curriculum[0, 0].Teachers[0]);
            Assert.Equal("����", curriculum[0, 0].Teachers[1]);
        }

        [Fact]
        public void Curriculum���e�X�g2()
        {
            var curriculum = new Curriculum(10, 10);
            var lesson = new Lesson("����", "1Q", "���j", "�P�N", "2��");
            curriculum[0, 0] = new CurriculumCell(lesson, new List<string> { "��{", "����", });

            var teacher = curriculum.ToTeacher();
            var lessonsSakamoto = teacher["��{"];
            Assert.Equal("����", lessonsSakamoto[0].Name);

            var lessonMatsui = teacher["����"];
            Assert.Equal("����", lessonMatsui[0].Name);

        }

    }
}