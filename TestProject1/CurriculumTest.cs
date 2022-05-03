using Model;
using System.Collections.Generic;
using Xunit;

namespace TestProject1
{
    public class CurriculumTest
    {
        [Fact]
        public void CurriculumπeXg()
        {
            var curriculum = new Curriculum(10, 10);
            var lesson = new Lesson("κ", "1Q", "j", "PN", "2ΐ");
            curriculum[0, 0] = new CurriculumCell(lesson, new List<string> { "β{", "Όδ", });

            Assert.Equal("κ", curriculum[0, 0].Lesson.Name);
            Assert.Equal("β{", curriculum[0, 0].Teachers[0]);
            Assert.Equal("Όδ", curriculum[0, 0].Teachers[1]);
        }

        [Fact]
        public void CurriculumπeXg2()
        {
            var curriculum = new Curriculum(10, 10);
            var κ = new Lesson("κ", "1Q", "j", "PN", "2ΐ");
            var Z = new Lesson("Z", "1Q", "Ξj", "PN", "3ΐ");
            var Θ = new Lesson("Θ", "1Q", "j", "PN", "4ΐ");
            var Πο = new Lesson("Πο", "1Q", "Ψj", "PN", "5ΐ");
            curriculum[0, 0] = new CurriculumCell(κ, new List<string> { "β{", "Όδ", });
            curriculum[0, 1] = new CurriculumCell(Z, new List<string> { "Όδ", });
            curriculum[0, 2] = new CurriculumCell(Θ, new List<string> { "Όδ", });
            curriculum[0, 3] = new CurriculumCell(Πο, new List<string> { "Όδ", });

            var teacher = curriculum.ToTeacher();
            var lessonsSakamoto = teacher["β{"];
            Assert.Equal("κ", lessonsSakamoto[0].Name);

            var lessonMatsui = teacher["Όδ"];
            Assert.Equal("κ", lessonMatsui[0].Name);
            Assert.Equal("Z", lessonMatsui[1].Name);
            Assert.Equal("Θ", lessonMatsui[2].Name);
            Assert.Equal("Πο", lessonMatsui[3].Name);

        }

    }
}