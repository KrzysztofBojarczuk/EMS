﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.APPLICATION.Dtos
{
    public class TaskCreateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; }
    }
}
