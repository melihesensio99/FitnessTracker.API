using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public ICollection<WorkoutProgram> WorkoutPrograms { get; set; } = new List<WorkoutProgram>();
        public ICollection<WorkoutLog> WorkoutLogs { get; set; } = new List<WorkoutLog>();
    }
}
