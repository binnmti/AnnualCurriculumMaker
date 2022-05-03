using Model;
using System.Collections.Generic;
using Xunit;

namespace TestProject1
{
    public class CurriculumTest
    {
        [Fact]
        public void Curriculum‚ğƒeƒXƒg()
        {
            var curriculum = new Curriculum(10, 10);
            var lesson = new Lesson("‘Œê", "1Q", "Œ—j", "‚P”N", "2ŒÀ");
            curriculum[0, 0] = new CurriculumCell(lesson, new List<string> { "â–{", "¼ˆä", });

            Assert.Equal("‘Œê", curriculum[0, 0].Lesson.Name);
            Assert.Equal("â–{", curriculum[0, 0].Teachers[0]);
            Assert.Equal("¼ˆä", curriculum[0, 0].Teachers[1]);
        }

        [Fact]
        public void Curriculum‚ğƒeƒXƒg2()
        {
            var curriculum = new Curriculum(10, 10);
            var ‘Œê = new Lesson("‘Œê", "1Q", "Œ—j", "‚P”N", "2ŒÀ");
            var Z” = new Lesson("Z”", "1Q", "‰Î—j", "‚P”N", "3ŒÀ");
            var —‰È = new Lesson("—‰È", "1Q", "…—j", "‚P”N", "4ŒÀ");
            var Ğ‰ï = new Lesson("Ğ‰ï", "1Q", "–Ø—j", "‚P”N", "5ŒÀ");
            curriculum[0, 0] = new CurriculumCell(‘Œê, new List<string> { "â–{", "¼ˆä", });
            curriculum[0, 1] = new CurriculumCell(Z”, new List<string> { "¼ˆä", });
            curriculum[0, 2] = new CurriculumCell(—‰È, new List<string> { "¼ˆä", });
            curriculum[0, 3] = new CurriculumCell(Ğ‰ï, new List<string> { "¼ˆä", });

            var teacher = curriculum.ToTeacher();
            var lessonsSakamoto = teacher["â–{"];
            Assert.Equal("‘Œê", lessonsSakamoto[0].Name);

            var lessonMatsui = teacher["¼ˆä"];
            Assert.Equal("‘Œê", lessonMatsui[0].Name);
            Assert.Equal("Z”", lessonMatsui[1].Name);
            Assert.Equal("—‰È", lessonMatsui[2].Name);
            Assert.Equal("Ğ‰ï", lessonMatsui[3].Name);

        }

    }
}