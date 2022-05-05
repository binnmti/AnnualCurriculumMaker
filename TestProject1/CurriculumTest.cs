using Model;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TestProject1
{
    public class CurriculumTest
    {
        [Fact]
        public void Curriculum���e�X�g()
        {
            var names = new List<string>() { "", "", "", "", "", "", "", "", "", "", };
            var curriculum = new Curriculum(10, 10, names, names);
            var lesson = new Lesson("����", "1Q���j", "�P�N2��");
            curriculum[0, 0] = new CurriculumCell(lesson, new List<string> { "��{", "����", });

            Assert.Equal("����", curriculum[0, 0].Lesson.Name);
            Assert.Equal("��{", curriculum[0, 0].Teachers[0]);
            Assert.Equal("����", curriculum[0, 0].Teachers[1]);
        }

        [Fact]
        public void ���t�d���`�F�b�N()
        {
            var names = new List<string>() { "", "", "", "", "", "", "", "", "", "", };
            var curriculum = new Curriculum(10, 10, names, names);
            var lesson = new Lesson("����", "1Q���j", "�P�N2��");
            curriculum[0, 0] = new CurriculumCell(lesson, new List<string> { "��{", "����", });
            Assert.False(curriculum.TryParse(0, 1, "����,��{", out var curriculumCell));
            Assert.False(curriculum.TryParse(0, 2, "����, ��{", out var curriculumCell2));
        }

        [Fact]
        public void Curriculum��Teacher�ɕϊ�()
        {
            var names = new List<string>() { "", "", "", "", "", "", "", "", "", "", };
            var curriculum = new Curriculum(10, 10, names, names);
            var ���� = new Lesson("����", "1Q���j", "�P�N2��");
            var �Z�� = new Lesson("�Z��", "1Q�Ηj", "�P�N3��");
            var ���� = new Lesson("����", "1Q���j", "�P�N4��");
            var �Љ� = new Lesson("�Љ�", "1Q�ؗj", "�P�N5��");
            curriculum[0, 0] = new CurriculumCell(����, new List<string> { "��{", "����", });
            curriculum[0, 1] = new CurriculumCell(�Z��, new List<string> { "����", });
            curriculum[0, 2] = new CurriculumCell(����, new List<string> { "����", });
            curriculum[0, 3] = new CurriculumCell(�Љ�, new List<string> { "����", });

            var teacher = curriculum.ToTeachers().ToList();
            Assert.Equal("����", teacher[0].Lessons[0].Name);

            Assert.Equal("����", teacher[1].Lessons[0].Name);
            Assert.Equal("�Z��", teacher[1].Lessons[1].Name);
            Assert.Equal("����", teacher[1].Lessons[2].Name);
            Assert.Equal("�Љ�", teacher[1].Lessons[3].Name);
        }
    }
}