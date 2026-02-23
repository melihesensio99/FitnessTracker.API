using Application.DTO.ProgramExercise;
using FluentValidation;

namespace Application.Validators.ProgramExercise
{
    public class UpdateProgramExerciseDtoValidator : AbstractValidator<UpdateProgramExerciseDto>
    {
        public UpdateProgramExerciseDtoValidator()
        {
            RuleFor(x => x.ExerciseId)
                .GreaterThan(0).WithMessage("Geçerli bir egzersiz ID zorunludur.");

            RuleFor(x => x.DayOfWeek)
                .IsInEnum().WithMessage("Geçersiz gün seçimi.");

            RuleFor(x => x.TargetSet)
                .GreaterThan(0).WithMessage("Hedef set sayısı 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(20).WithMessage("Hedef set sayısı 20'den fazla olamaz.");

            RuleFor(x => x.TargetRep)
                .GreaterThan(0).WithMessage("Hedef tekrar sayısı 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(100).WithMessage("Hedef tekrar sayısı 100'den fazla olamaz.");

            RuleFor(x => x.RestTime)
                .GreaterThanOrEqualTo(0).WithMessage("Dinlenme süresi negatif olamaz.")
                .LessThanOrEqualTo(600).WithMessage("Dinlenme süresi 600 saniyeden fazla olamaz.");

            RuleFor(x => x.WeekNumber)
                .GreaterThan(0).WithMessage("Hafta numarası 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(52).WithMessage("Hafta numarası 52'den fazla olamaz.");

            RuleFor(x => x.TargetWeightPercent)
                .InclusiveBetween(0, 200).WithMessage("Hedef ağırlık yüzdesi 0-200 arasında olmalıdır.")
                .When(x => x.TargetWeightPercent.HasValue);
        }
    }
}
