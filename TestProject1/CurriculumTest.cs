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
            var lesson = new Lesson("国語", "1Q", "月曜", "１年", "2限");
            curriculum[0, 0] = new CurriculumCell(lesson, new List<string> { "坂本", "松井", });

            Assert.Equal("国語", curriculum[0, 0].Lesson.Name);
            Assert.Equal("坂本", curriculum[0, 0].Teachers[0]);
            Assert.Equal("松井", curriculum[0, 0].Teachers[1]);
        }

        [Fact]
        public void Curriculumをテスト2()
        {
            var curriculum = new Curriculum(10, 10);
            var lesson = new Lesson("国語", "1Q", "月曜", "１年", "2限");
            curriculum[0, 0] = new CurriculumCell(lesson, new List<string> { "坂本", "松井", });

            var teacher = curriculum.ToTeacher();
            var lessonsSakamoto = teacher["坂本"];
            Assert.Equal("国語", lessonsSakamoto[0].Name);

            var lessonMatsui = teacher["松井"];
            Assert.Equal("国語", lessonMatsui[0].Name);

        }

    }
}