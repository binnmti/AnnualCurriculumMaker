using Model;
using System.Collections.Generic;
using Xunit;

namespace TestProject1
{
    public class CurriculumTest
    {
        [Fact]
        public void Curriculum๐eXg()
        {
            var curriculum = new Curriculum(10, 10);
            var lesson = new Lesson("๊", "1Q", "j", "PN", "2ภ");
            curriculum[0, 0] = new CurriculumCell(lesson, new List<string> { "โ{", "ผไ", });

            Assert.Equal("๊", curriculum[0, 0].Lesson.Name);
            Assert.Equal("โ{", curriculum[0, 0].Teachers[0]);
            Assert.Equal("ผไ", curriculum[0, 0].Teachers[1]);
        }

        [Fact]
        public void Curriculum๐eXg2()
        {
            var curriculum = new Curriculum(10, 10);
            var lesson = new Lesson("๊", "1Q", "j", "PN", "2ภ");
            curriculum[0, 0] = new CurriculumCell(lesson, new List<string> { "โ{", "ผไ", });

            var teacher = curriculum.ToTeacher();
            var lessonsSakamoto = teacher["โ{"];
            Assert.Equal("๊", lessonsSakamoto[0].Name);

            var lessonMatsui = teacher["ผไ"];
            Assert.Equal("๊", lessonMatsui[0].Name);

        }

    }
}