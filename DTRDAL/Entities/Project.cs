﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DTRDAL.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Title { get; set; }
        public int? WantedSupervisorId { get; set; }
        public Supervisor WantedSupervisor { get; set; }
        public int? AssignedSupervisorId { get; set; }
        public Supervisor AssignedSupervisor { get; set; }
    }
}
