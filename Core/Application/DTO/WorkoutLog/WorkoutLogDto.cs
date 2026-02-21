using System;

namespace Application.DTO.WorkoutLog
{
    public class WorkoutLogDto
    {
        public int Id { get; set; }
        public int ProgramExerciseId { get; set; }
        
        // Ekranda "Hangi hareketi yapmıştım?" göstermek için pratik bir isim alanı.
        public string ExerciseName { get; set; } 
        
        public int ActualSet { get; set; }
        public int ActualRep { get; set; }
        public decimal ActualWeight { get; set; }
        public DateTime Date { get; set; }
        public bool IsSuccess { get; set; }
    }
}
