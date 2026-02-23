using Application.DTO.Exercise;
using Application.Exceptions;
using Application.Abstraction.Services;
using Application.Abstraction.UnitOfWorks;
using Application.Repositories.EntitiesRepository;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ExerciseService(IExerciseRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ExerciseDto> AddExerciseAsync(CreateExerciseDto createDto)
        {
            var mapping = _mapper.Map<Exercise>(createDto);
            await _repository.AddAsync(mapping);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<ExerciseDto>(mapping);
            return result;
        }

        public async Task<bool> DeleteExerciseAsync(int exerciseId)
        {
            var exercise = await _repository.GetByIdAsync(exerciseId);
            if (exercise == null)
            {
                throw new NotFoundException(nameof(Exercise), exerciseId);
            }
            _repository.Remove(exercise);
            await _unitOfWork.SaveAsync();
            return true;

        }

        public async Task<List<ExerciseDto>> GetAllExercisesAsync()
        {
            var exercises = _repository.GetAll();
            if (exercises == null)
            {
                throw new NotFoundException(nameof(Exercise), "No exercises found.");
            }
            var mapping = _mapper.Map<List<ExerciseDto>>(exercises);
            return mapping;
        }

        public async Task<ExerciseDto> GetExerciseByIdAsync(int exerciseId)
        {
            var exercise = await _repository.GetByIdAsync(exerciseId);
            if (exercise == null)
            {
                throw new NotFoundException(nameof(Exercise), exerciseId);
            }
            var mapping = _mapper.Map<ExerciseDto>(exercise);
            return mapping;
        }

        public async Task<ExerciseDto> GetExercisesByMuscleGroup(string muscleGroup)
        {
            var exerciseWithMuscleGroup = await _repository.GetExercisesByMuscleGroup(muscleGroup);
            if (exerciseWithMuscleGroup == null)
            {
                throw new NotFoundException(nameof(Exercise), $"No exercise found for muscle group: {muscleGroup}");
            }
            var mapping = _mapper.Map<ExerciseDto>(exerciseWithMuscleGroup);
            return mapping;
        }

        public async Task<List<ExerciseDto>> SearchExercisesByNameAsync(string name)
        {
            var searchByName = await _repository.SearchExercisesByNameAsync(name);
            if (searchByName == null)
            {
                throw new NotFoundException(nameof(Exercise), $"No exercise found with name containing: {name}");
            }
            var mapping = _mapper.Map<List<ExerciseDto>>(searchByName);
            return mapping;
        }

        public async Task<bool> UpdateExerciseAsync(UpdateExerciseDto updateDto)
        {
            var exercise = await _repository.GetByIdAsync(updateDto.Id);
            if (exercise == null)
            {
                throw new NotFoundException(nameof(Exercise), updateDto.Id);
            }
            var mapping = _mapper.Map(updateDto, exercise);
            _repository.Update(mapping);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
