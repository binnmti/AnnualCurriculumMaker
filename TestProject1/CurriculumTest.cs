using AnnualCurriculumMaker;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TestProject1
{
    public class CurriculumTest
    {
        [Fact]
        public void CurriculumπeXg()
        {
            var names = new List<string>() { "", "", "", "", "", "", "", "", "", "", };
            var curriculum = new Curriculum(10, 10, names, names, names, names);
            var lesson = new Lesson("κ", "1Q", "j", "PN", "2ΐ");
            curriculum[0, 0] = new CurriculumCell(lesson, new List<string> { "β{", "Όδ", }, 0);

            Assert.Equal("κ", curriculum[0, 0].Lesson.Name);
            Assert.Equal("β{", curriculum[0, 0].Teachers[0]);
            Assert.Equal("Όδ", curriculum[0, 0].Teachers[1]);
        }

        //[Fact]
        //public void ³td‘`FbN()
        //{
        //    var names = new List<string>() { "", "", "", "", "", "", "", "", "", "", };
        //    var curriculum = new Curriculum(10, 10, names, names, names, names);
        //    var lesson = new Lesson("κ", "1Q","j", "PN","2ΐ");
        //    curriculum[0, 0] = new CurriculumCell(lesson, new List<string> { "β{", "Όδ", });
        //    Assert.False(curriculum.TryParse(0, 1, "κ,β{", out var curriculumCell));
        //    Assert.False(curriculum.TryParse(0, 2, "κ, β{", out var curriculumCell2));
        //}

        [Fact]
        public void CurriculumπTeacherΙΟ·()
        {
            var names = new List<string>() { "", "", "", "", "", "", "", "", "", "", };
            var curriculum = new Curriculum(10, 10, names, names, names, names);
            var κ = new Lesson("κ", "1Q","j", "PN","2ΐ");
            var Z = new Lesson("Z", "1Q", "Ξj", "PN", "3ΐ");
            var Θ = new Lesson("Θ", "1Q", "j", "PN", "4ΐ");
            var Πο = new Lesson("Πο", "1Q", "Ψj", "PN", "5ΐ");
            curriculum[0, 0] = new CurriculumCell(κ, new List<string> { "β{", "Όδ", }, 0);
            curriculum[0, 1] = new CurriculumCell(Z, new List<string> { "Όδ", }, 0);
            curriculum[0, 2] = new CurriculumCell(Θ, new List<string> { "Όδ", }, 0);
            curriculum[0, 3] = new CurriculumCell(Πο, new List<string> { "Όδ", }, 0);

            var teacher = curriculum.ToTeachers().ToList();
            Assert.Equal("κ", teacher[0].Lessons[0].Name);
            Assert.Equal("κ", teacher[1].Lessons[0].Name);
            Assert.Equal("Z", teacher[1].Lessons[1].Name);
            Assert.Equal("Θ", teacher[1].Lessons[2].Name);
            Assert.Equal("Πο", teacher[1].Lessons[3].Name);
        }
    }
}