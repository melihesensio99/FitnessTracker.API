using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class WorkoutProgram
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Title { get; set; }
        public string Level { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public string Ambition { get; set; }
        public bool IsPublic { get; set; }
        public bool IsActive { get; set; }
        public User User { get; set; }
        public ICollection<ProgramExercise> ProgramExercises { get; set; } = new List<ProgramExercise>();

        public WorkoutProgram CloneForUser(int userId)
        {
            return new WorkoutProgram
            {
                UserId = userId,
                Title = this.Title,
                Description = this.Description,
                Level = this.Level,
                Ambition = this.Ambition,
                ImageUrl = this.ImageUrl,
                IsPublic = false,
                ProgramExercises = this.ProgramExercises?.Select(pe => new ProgramExercise
                {
                    ExerciseId = pe.ExerciseId,
                    DayOfWeek = pe.DayOfWeek,
                    TargetSet = pe.TargetSet,
                    TargetRep = pe.TargetRep,
                    RestTime = pe.RestTime,
                    WeekNumber = pe.WeekNumber,
                    TargetWeightPercent = pe.TargetWeightPercent
                }).ToList() ?? new List<ProgramExercise>()
            };
        }
    }
}
