using Domain.Entities.Community;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public ICollection<WorkoutProgram> WorkoutPrograms { get; set; } = new List<WorkoutProgram>();
        public ICollection<WorkoutLog> WorkoutLogs { get; set; } = new List<WorkoutLog>();
        
    }
}
