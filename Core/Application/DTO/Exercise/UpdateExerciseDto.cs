using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO.Exercise
{
    public class UpdateExerciseDto
    {
        public int Id { get; set; }
        public string ExerciseName { get; set; }
        public string TargetMuscle { get; set; }
        public string VideoUrl { get; set; }
    }
}
