using Model;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TestProject1
{
    public class CurriculumTest
    {
        [Fact]
        public void Curriculum‚ğƒeƒXƒg()
        {
            var names = new List<string>() { "", "", "", "", "", "", "", "", "", "", };
            var curriculum = new Curriculum(10, 10, names, names);
            var lesson = new Lesson("‘Œê", "1QŒ—j", "‚P”N2ŒÀ");
            curriculum[0, 0] = new CurriculumCell(lesson, new List<string> { "â–{", "¼ˆä", });

            Assert.Equal("‘Œê", curriculum[0, 0].Lesson.Name);
            Assert.Equal("â–{", curriculum[0, 0].Teachers[0]);
            Assert.Equal("¼ˆä", curriculum[0, 0].Teachers[1]);
        }

        [Fact]
        public void ‹³td•¡ƒ`ƒFƒbƒN()
        {
            var names = new List<string>() { "", "", "", "", "", "", "", "", "", "", };
            var curriculum = new Curriculum(10, 10, names, names);
            var lesson = new Lesson("‘Œê", "1QŒ—j", "‚P”N2ŒÀ");
            curriculum[0, 0] = new CurriculumCell(lesson, new List<string> { "â–{", "¼ˆä", });
            Assert.False(curriculum.TryParse(0, 1, "‘Œê,â–{", out var curriculumCell));
            Assert.False(curriculum.TryParse(0, 2, "‘Œê, â–{", out var curriculumCell2));
        }

        [Fact]
        public void Curriculum‚ğTeacher‚É•ÏŠ·()
        {
            var names = new List<string>() { "", "", "", "", "", "", "", "", "", "", };
            var curriculum = new Curriculum(10, 10, names, names);
            var ‘Œê = new Lesson("‘Œê", "1QŒ—j", "‚P”N2ŒÀ");
            var Z” = new Lesson("Z”", "1Q‰Î—j", "‚P”N3ŒÀ");
            var —‰È = new Lesson("—‰È", "1Q…—j", "‚P”N4ŒÀ");
            var Ğ‰ï = new Lesson("Ğ‰ï", "1Q–Ø—j", "‚P”N5ŒÀ");
            curriculum[0, 0] = new CurriculumCell(‘Œê, new List<string> { "â–{", "¼ˆä", });
            curriculum[0, 1] = new CurriculumCell(Z”, new List<string> { "¼ˆä", });
            curriculum[0, 2] = new CurriculumCell(—‰È, new List<string> { "¼ˆä", });
            curriculum[0, 3] = new CurriculumCell(Ğ‰ï, new List<string> { "¼ˆä", });

            var teacher = curriculum.ToTeachers().ToList();
            Assert.Equal("‘Œê", teacher[0].Lessons[0].Name);

            Assert.Equal("‘Œê", teacher[1].Lessons[0].Name);
            Assert.Equal("Z”", teacher[1].Lessons[1].Name);
            Assert.Equal("—‰È", teacher[1].Lessons[2].Name);
            Assert.Equal("Ğ‰ï", teacher[1].Lessons[3].Name);
        }
    }
}