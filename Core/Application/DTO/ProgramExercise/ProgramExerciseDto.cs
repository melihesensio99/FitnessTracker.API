using System;

namespace Application.DTO.ProgramExercise
{
    public class ProgramExerciseDto
    {
        public int Id { get; set; }
        
        // Sadece DTO veya basit tip taşıyoruz. Entity'leri (Exercise tablosunu) gizliyoruz.
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; } // Ekranda göstermek pratik olsun diye (Join ile alınacak)
        
        public DayOfWeek DayOfWeek { get; set; }
        public int TargetSet { get; set; }
        public int TargetRep { get; set; }
        public int RestTime { get; set; }
        public int WeekNumber { get; set; }
        public decimal? TargetWeightPercent { get; set; }
    }
}
