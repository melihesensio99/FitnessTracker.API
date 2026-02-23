using Application.DTO.WorkoutProgram;
using Application.Abstraction.Services;
using Application.Abstraction.UnitOfWorks;
using Application.Repositories.EntitiesRepository;
using Application.Abstraction.Storage;
using Application.Helpers;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Application.Exceptions;
using Application.Common.Pagination;

namespace Infrastructure.Services
{
    public class WorkoutProgramService : IWorkoutProgramService
    {
        private readonly IWorkoutProgramRepository _repository;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;

        public WorkoutProgramService(
            IWorkoutProgramRepository repository,
            IExerciseRepository exerciseRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IStorageService storageService)
        {
            _repository = repository;
            _exerciseRepository = exerciseRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
        }

        public async Task<WorkoutProgramDto> AddWorkoutProgramAsync(CreateWorkoutProgramDto createDto)
        {
            if (createDto == null) throw new ArgumentNullException(nameof(createDto));

            if (createDto.ProgramExercises != null && createDto.ProgramExercises.Any())
            {
                foreach (var exercise in createDto.ProgramExercises)
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
            var result = _mapper.Map<WorkoutProgramDto>(mapping);
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

            var userPrograms = await _repository.GetUserProgramsAsync(userId);
            if (userPrograms.Any(p => p.Title == originalProgram.Title))
            {
                throw new ValidationException("Bu programa zaten katıldınız (kütüphanenizde mevcut).");
            }

            var clonedProgram = originalProgram.CloneForUser(userId);

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

            if (!string.IsNullOrEmpty(program.ImageUrl))
            {
                await MediaHelper.DeleteMediaAsync(program.ImageUrl, _storageService);
            }

            _repository.Remove(program);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<List<WorkoutProgramDto>> GetUserProgramsAsync(int userId)
        {
            var userPrograms = await _repository.GetUserProgramsAsync(userId);
            var resultDto = _mapper.Map<List<WorkoutProgramDto>>(userPrograms);
            return resultDto;
        }

        public async Task<WorkoutProgramDto> GetWorkoutProgramDetailByIdAsync(int Id)
        {
            var program = await _repository.GetWorkoutProgramDetailWithExercisesAsync(Id);

            var resultDto = _mapper.Map<WorkoutProgramDto>(program);

            return resultDto;
        }
        public async Task<bool> UpdateWorkoutProgramAsync(UpdateWorkoutProgramDto updateDto)
        {
            if (updateDto == null) throw new ValidationException("Güncellenecek veri boş olamaz.");
            var existingProgram = await _repository.GetWorkoutProgramDetailWithExercisesAsync(updateDto.Id);

            if (existingProgram == null)
            {
                throw new NotFoundException(nameof(WorkoutProgram), updateDto.Id);
            }

            var oldImageUrl = existingProgram.ImageUrl;

            _mapper.Map(updateDto, existingProgram);

            
            if (oldImageUrl != existingProgram.ImageUrl && !string.IsNullOrEmpty(oldImageUrl))
            {
              
                 await MediaHelper.DeleteMediaAsync(oldImageUrl, _storageService);
            }

            if (updateDto.ProgramExercises != null)
            {
                var exercisesToRemove = existingProgram.ProgramExercises
                    .Where(pe => !updateDto.ProgramExercises.Any(dto => dto.Id == pe.Id))
                    .ToList();
                foreach (var exerciseToRemove in exercisesToRemove)
                {
                    existingProgram.ProgramExercises.Remove(exerciseToRemove);
                }

                foreach (var exerciseDto in updateDto.ProgramExercises)
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

        public async Task<PagedResponse<WorkoutProgramDto>> GetFilteredProgramsAsync(WorkoutProgramFilteredDto filter)
        {
            var pagedEntities = await _repository.GetFilteredProgramsAsync(filter);

            var dtoList = _mapper.Map<List<WorkoutProgramDto>>(pagedEntities.Data);

            return new PagedResponse<WorkoutProgramDto>
            {
                Data = dtoList,
                TotalCount = pagedEntities.TotalCount,
                CurrentPage = pagedEntities.CurrentPage,
                PageSize = pagedEntities.PageSize,
                TotalPages = (int)Math.Ceiling(pagedEntities.TotalCount / (double)pagedEntities.PageSize)
            };
        }

        public async Task<bool> ActivateProgramAsync(int programId, int userId)
        {
            var success = await _repository.ActivateProgramByUserIdAsync(programId, userId);
            
            if (!success)
            {
                throw new NotFoundException(nameof(WorkoutProgram), programId);
            }

            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
