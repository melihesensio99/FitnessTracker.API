using Application.DTO.Exercise;
using Application.DTO.ProgramExercise;
using Application.DTO.WorkoutLog;
using Application.DTO.WorkoutProgram;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            // WorkoutProgram Eşleşmeleri
            CreateMap<WorkoutProgram, WorkoutResultDto>().ReverseMap();
            CreateMap<WorkoutProgram, CreateWorkoutProgramDto>().ReverseMap();
            CreateMap<WorkoutProgram, UpdateWorkoutProgramDto>().ReverseMap();

            // ProgramExercise Eşleşmeleri
            CreateMap<ProgramExercise, ProgramExerciseDto>()
                // ExerciseName gibi DTO'da olup da Entity'de bir alt objede (Exercise tablosunda) 
                // bulunan verileri böyle otomatik eşleyebiliriz (İleri seviyedir, şimdilik dursun).
                .ForMember(dest => dest.ExerciseName, opt => opt.MapFrom(src => src.Exercise != null ? src.Exercise.ExerciseName : null))
                .ReverseMap();
            CreateMap<ProgramExercise, CreateProgramExerciseDto>().ReverseMap();
            CreateMap<ProgramExercise, UpdateProgramExerciseDto>().ReverseMap();

            // Exercise Eşleşmeleri
            CreateMap<Exercise, ExerciseDto>().ReverseMap();
            CreateMap<Exercise, CreateExerciseDto>().ReverseMap();

            // WorkoutLog Eşleşmeleri
            CreateMap<WorkoutLog, WorkoutLogDto>().ReverseMap();
            CreateMap<WorkoutLog, CreateWorkoutLogDto>().ReverseMap();
        }
    }
}
