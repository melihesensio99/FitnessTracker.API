using Application.Abstraction.Services;
using Application.Abstraction.UnitOfWorks;
using Application.DTO.WorkoutLog;
using Application.Repositories.EntitiesRepository;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Services
{
    public class WorkoutLogService : IWorkoutLogService
    {
        private readonly IWorkoutLogRepository _workoutLogRepository;
        private readonly IProgramExerciseRepository _programExerciseRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public WorkoutLogService(IWorkoutLogRepository workoutLogRepository, IProgramExerciseRepository programExerciseRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _workoutLogRepository = workoutLogRepository;
            _programExerciseRepository = programExerciseRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<WorkoutLogDto> AddWorkoutLogAsync(CreateWorkoutLogDto createDto)
        {
            var workoutLog = _mapper.Map<WorkoutLog>(createDto);

            var targetExercise = await _programExerciseRepository.GetByIdAsync(workoutLog.ProgramExerciseId);

            if (targetExercise == null)
               throw new Exception("Böyle bir egzersiz programı bulunamadı!");

            if (workoutLog.ActualSet > targetExercise.TargetSet)
            {
                throw new Exception($"Geçersiz set numarası. Girdiğiniz set ({workoutLog.ActualSet}), en fazla hedef set ({targetExercise.TargetSet}) kadar olabilir.");
            }

            var startDate = workoutLog.Date.Date;
            var endDate = startDate.AddDays(1).AddTicks(-1);
            var todayLogs = await _workoutLogRepository.GetLogsByDateRangeAsync(workoutLog.UserId, startDate, endDate);
            
            bool isAlreadyLogged = todayLogs.Any(l => l.ProgramExerciseId == workoutLog.ProgramExerciseId && l.ActualSet == workoutLog.ActualSet);
            if (isAlreadyLogged)
            {
                throw new Exception($"{workoutLog.ActualSet}. set bugün zaten kaydedilmiş. Lütfen farklı bir seti kaydedin veya yarın tekrar deneyin.");
            }

            bool isRepSuccess = workoutLog.ActualRep >= targetExercise.TargetRep;

            workoutLog.Status = isRepSuccess ? Domain.Enums.WorkoutStatus.Success : Domain.Enums.WorkoutStatus.RepMissing;

            if (createDto.Status == Domain.Enums.WorkoutStatus.Failure)
            {
                workoutLog.Status = Domain.Enums.WorkoutStatus.Failure;
            }

            await _workoutLogRepository.AddAsync(workoutLog);
            await _unitOfWork.SaveAsync();
            
            return _mapper.Map<WorkoutLogDto>(workoutLog);
        }

        public async Task<List<WorkoutLogDto>> GetExerciseLogsAsync(int programExerciseId)
        {
            var logs = await _workoutLogRepository.GetExerciseLogsAsync(programExerciseId);
            return _mapper.Map<List<WorkoutLogDto>>(logs);
        }

        public async Task<List<WorkoutLogDto>> GetLogsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var logs = await _workoutLogRepository.GetLogsByDateRangeAsync(userId, startDate, endDate);
            return _mapper.Map<List<WorkoutLogDto>>(logs);
        }

        public async Task<WorkoutLogDto?> GetUserLastLogAsync(int userId, int programExerciseId)
        {
            var lastLog = await _workoutLogRepository.GetUserLastLogAsync(userId, programExerciseId);
            return _mapper.Map<WorkoutLogDto?>(lastLog);
        }

        public async Task<List<WorkoutLogDto>> GetUserLogsAsync(int userId)
        {
            var userLogs = await _workoutLogRepository.GetUserLogsAsync(userId);
            return _mapper.Map<List<WorkoutLogDto>>(userLogs);
        }
    }
}
