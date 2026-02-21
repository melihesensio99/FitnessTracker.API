using Application.DTO.WorkoutProgram;
using Application.Interfaces.Services;
using Application.Interfaces.UnitOfWorks;
using Application.Repositories.EntitiesRepository;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Application.Exceptions;

namespace Infrastructure.Services
{
    public class WorkoutProgramService : IWorkoutProgramService
    {
        private readonly IWorkoutProgramRepository _repository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public WorkoutProgramService(
            IWorkoutProgramRepository repository, 
            IExerciseRepository exerciseRepository,
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _exerciseRepository = exerciseRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<WorkoutResultDto> AddWorkoutProgramAsync(CreateWorkoutProgramDto createDto)
        {
            if (createDto == null) throw new ArgumentNullException(nameof(createDto));

            if (createDto.Exercises != null && createDto.Exercises.Any())
            {
                foreach (var exercise in createDto.Exercises)
                {
                    var existingExercise = await _exerciseRepository.GetByIdAsync(exercise.ExerciseId);
                    if (existingExercise == null)
                    {
                        throw new NotFoundException(nameof(Exercise), exercise.ExerciseId);
                    }
                }
            }

            var mapping = _mapper.Map<WorkoutProgram>(createDto);
            await _repository.AddAsync(mapping);
            await _unitOfWork.SaveAsync();
            var result = _mapper.Map<WorkoutResultDto>(mapping);
            return result;
        }

        public async Task<int> CloneSystemProgramToUserAsync(int programId, int userId) // navigator.navigate("ProgramDetailScreen", { programId: 1048 }) for frontend
        {
            var originalProgram = await _repository.GetWorkoutProgramDetailWithExercisesAsync(programId);

            if (originalProgram == null)
            {
                throw new NotFoundException(nameof(WorkoutProgram), programId);
            }

            if (!originalProgram.IsPublic && originalProgram.UserId != userId)
            {
                throw new ValidationException("Bu program gizli veya size ait olmadığı için kopyalanamaz.");
            }

            var clonedProgram = _mapper.Map<WorkoutProgram>(originalProgram);


            clonedProgram.UserId = userId;
            clonedProgram.IsPublic = false;

            await _repository.AddAsync(clonedProgram);
            await _unitOfWork.SaveAsync();

            return clonedProgram.Id;
        }

        public async Task<bool> DeleteWorkoutProgramAsync(int programId)
        {
            var program = await _repository.GetByIdAsync(programId);
            if (program == null)
            {
                throw new NotFoundException(nameof(WorkoutProgram), programId);
            }

            _repository.Remove(program);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<List<WorkoutResultDto>> GetSystemWorkoutProgramsAsync()
        {
            var systemPrograms = await _repository.GetSystemWorkoutProgramsAsync();
            return _mapper.Map<List<WorkoutResultDto>>(systemPrograms);
        }

        public async Task<List<WorkoutResultDto>> GetUserWorkoutProgramsAsync(int userId)
        {
            var userProgram = await _repository.GetUserWorkoutProgramsAsync(userId);
            var resultDto = _mapper.Map<List<WorkoutResultDto>>(userProgram);
            return resultDto;

        }

        public async Task<WorkoutResultDto> GetWorkoutProgramDetailByIdAsync(int Id)
        {
            var program = await _repository.GetWorkoutProgramDetailWithExercisesAsync(Id);

            var resultDto = _mapper.Map<WorkoutResultDto>(program);

            return resultDto;
        }

        public async Task<bool> IsProgramPublic(int programId)
        {
            var program = await _repository.GetByIdAsync(programId);
            if (program == null)
            {
                return false;
            }
            return program.IsPublic;
        }

        public async Task<bool> UpdateWorkoutProgramAsync(UpdateWorkoutProgramDto updateDto)
        {
            if (updateDto == null) throw new ValidationException("Güncellenecek veri boş olamaz.");
            var existingProgram = await _repository.GetWorkoutProgramDetailWithExercisesAsync(updateDto.Id);

            if (existingProgram == null)
            {
                throw new NotFoundException(nameof(WorkoutProgram), updateDto.Id);
            }
           
            _mapper.Map(updateDto, existingProgram);
          
            if (updateDto.Exercises != null)
            {
                var exercisesToRemove = existingProgram.ProgramExercises
                    .Where(pe => !updateDto.Exercises.Any(dto => dto.Id == pe.Id))
                    .ToList();
                foreach (var exerciseToRemove in exercisesToRemove)
                {
                    existingProgram.ProgramExercises.Remove(exerciseToRemove);
                }
                
                foreach (var exerciseDto in updateDto.Exercises)
                {
                    var existingExerciseDef = await _exerciseRepository.GetByIdAsync(exerciseDto.ExerciseId);
                    if (existingExerciseDef == null)
                    {
                        throw new NotFoundException(nameof(Exercise), exerciseDto.ExerciseId);
                    }

                    if (exerciseDto.Id == null || exerciseDto.Id == 0)
                    {
                        var newExercise = _mapper.Map<ProgramExercise>(exerciseDto);
                        existingProgram.ProgramExercises.Add(newExercise);
                    }
                    else
                    {
                        var existingExercise = existingProgram.ProgramExercises.FirstOrDefault(pe => pe.Id == exerciseDto.Id);
                        if (existingExercise != null)
                        {
                            _mapper.Map(exerciseDto, existingExercise);
                        }
                    }
                }
            }
            _repository.Update(existingProgram);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
