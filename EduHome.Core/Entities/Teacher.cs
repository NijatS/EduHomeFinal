using EduHome.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class Teacher:BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int ExperienceYear { get; set; }
        [Required]
        public int DegreeId { get; set; }
        [Required]
        public int PositionId { get; set; }
        public string? Image { get ; set; }
        [NotMapped]
        public IFormFile file { get; set; } 
        public Degree? Degree { get; set; }
        public Position? Position { get; set; } 
        public List<Skill>? Skills { get; set; }
        public List<Social>? Socials { get; set; }
        public List<TeacherHobby>? teacherHobbies { get; set; }
    }
}
