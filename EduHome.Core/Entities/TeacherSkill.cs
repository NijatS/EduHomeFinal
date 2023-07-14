using EduHome.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class TeacherSkill:BaseModel
    {
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public int SkillId { get; set; }
        public Skill? Skill { get; set; }
        [Required]
        public int Degree { get; set; }
    }
}
