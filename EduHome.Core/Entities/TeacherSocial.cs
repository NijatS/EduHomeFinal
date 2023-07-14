using EduHome.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduHome.Core.Entities
{
    public class TeacherSocial:BaseModel
    {
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public int SocialId { get; set; }
        public Social? Social { get; set; }
        [Required]
        public string Link { get; set; }
    }
}
