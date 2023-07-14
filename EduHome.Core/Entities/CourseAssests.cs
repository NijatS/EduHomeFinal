﻿using EduHome.Core.Entities;
using EduHome.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fir.Core.Entities
{
	public class CourseAssests:BaseModel
	{
		[Required]
		public string Name { get; set; }
		public List<Course>? Courses { get; set; }
	}
}