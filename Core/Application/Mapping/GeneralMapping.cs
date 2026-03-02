using Application.DTO.Exercise;
using Application.DTO.ProgramExercise;
using Application.DTO.WorkoutLog;
using Application.DTO.WorkoutProgram;
using AutoMapper;
using Domain.Entities;
using System.Linq;

namespace Application.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
           
            CreateMap<WorkoutProgram, WorkoutProgramDto>().ReverseMap();
            CreateMap<WorkoutProgram, CreateWorkoutProgramDto>().ReverseMap();
            CreateMap<WorkoutProgram, UpdateWorkoutProgramDto>().ReverseMap()
                .ForMember(dest => dest.ProgramExercises, opt => opt.Ignore());

            
            CreateMap<ProgramExercise, ProgramExerciseDto>()
                .ForMember(dest => dest.ExerciseName, opt => opt.MapFrom(src => src.Exercise != null ? src.Exercise.ExerciseName : null))
                .ReverseMap();
            CreateMap<ProgramExercise, CreateProgramExerciseDto>().ReverseMap();
            CreateMap<ProgramExercise, UpdateProgramExerciseDto>().ReverseMap();

       
            CreateMap<Exercise, ExerciseDto>().ReverseMap();
            CreateMap<Exercise, CreateExerciseDto>().ReverseMap();

         
            CreateMap<WorkoutLog, WorkoutLogDto>()
                .ForMember(dest => dest.ExerciseName, opt => opt.MapFrom(src => src.ProgramExercise != null && src.ProgramExercise.Exercise != null ? src.ProgramExercise.Exercise.ExerciseName : null))
                .ForMember(dest => dest.programExerciseDto, opt => opt.MapFrom(src => src.ProgramExercise))
                .ReverseMap();
            CreateMap<WorkoutLog, CreateWorkoutLogDto>()
                .ForMember(dest => dest.ActualWeight, opt => opt.MapFrom(src => src.Weight))
                .ReverseMap()
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.ActualWeight));

            // Community & Post Mappings
            CreateMap<Domain.Entities.Community.Post, Application.DTO.Community.Post.CreatePostDto>().ReverseMap();
            CreateMap<Domain.Entities.Community.Post, Application.DTO.Community.Post.UpdatePostDto>().ReverseMap();
            
            CreateMap<Domain.Entities.Community.Post, Application.DTO.Community.Post.ResultPostDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null))
                .ForMember(dest => dest.MediaUrls, opt => opt.MapFrom(src => src.Media.Select(m => m.Url).ToList()));

            // Comment Mappings
            CreateMap<Domain.Entities.Community.Comment, Application.DTO.Community.Comment.CreateCommentDto>().ReverseMap();
            CreateMap<Domain.Entities.Community.Comment, Application.DTO.Community.Comment.ResultCommentDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null));
        }
    }
}
