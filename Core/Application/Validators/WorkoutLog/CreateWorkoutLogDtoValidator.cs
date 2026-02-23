using Application.DTO.WorkoutLog;
using FluentValidation;

namespace Application.Validators.WorkoutLog
{
    public class CreateWorkoutLogDtoValidator : AbstractValidator<CreateWorkoutLogDto>
    {
        public CreateWorkoutLogDtoValidator()
        {
            RuleFor(x => x.ProgramExerciseId)
                .GreaterThan(0).WithMessage("Geçerli bir program egzersiz ID zorunludur.");

            RuleFor(x => x.ActualSet)
                .GreaterThan(0).WithMessage("Yapılan set sayısı 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(20).WithMessage("Yapılan set sayısı 20'den fazla olamaz.");

            RuleFor(x => x.ActualRep)
                .GreaterThan(0).WithMessage("Yapılan tekrar sayısı 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(100).WithMessage("Yapılan tekrar sayısı 100'den fazla olamaz.");

            RuleFor(x => x.ActualWeight)
                .GreaterThanOrEqualTo(0).WithMessage("Ağırlık negatif olamaz.")
                .LessThanOrEqualTo(1000).WithMessage("Ağırlık 1000 kg'dan fazla olamaz.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Antrenman tarihi zorunludur.")
                .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1)).WithMessage("Gelecek tarihli log eklenemez.");
        }
    }
}
