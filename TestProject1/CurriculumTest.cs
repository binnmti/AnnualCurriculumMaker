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
            var ���� = new Lesson("����", "1Q", "���j", "�P�N", "2��");
            var �Z�� = new Lesson("�Z��", "1Q", "�Ηj", "�P�N", "3��");
            var ���� = new Lesson("����", "1Q", "���j", "�P�N", "4��");
            var �Љ� = new Lesson("�Љ�", "1Q", "�ؗj", "�P�N", "5��");
            curriculum[0, 0] = new CurriculumCell(����, new List<string> { "��{", "����", });
            curriculum[0, 1] = new CurriculumCell(�Z��, new List<string> { "����", });
            curriculum[0, 2] = new CurriculumCell(����, new List<string> { "����", });
            curriculum[0, 3] = new CurriculumCell(�Љ�, new List<string> { "����", });

            var teacher = curriculum.ToTeacher();
            var lessonsSakamoto = teacher["��{"];
            Assert.Equal("����", lessonsSakamoto[0].Name);

            var lessonMatsui = teacher["����"];
            Assert.Equal("����", lessonMatsui[0].Name);
            Assert.Equal("�Z��", lessonMatsui[1].Name);
            Assert.Equal("����", lessonMatsui[2].Name);
            Assert.Equal("�Љ�", lessonMatsui[3].Name);

        }

    }
}