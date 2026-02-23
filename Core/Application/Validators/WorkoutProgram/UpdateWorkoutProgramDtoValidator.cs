using Application.DTO.WorkoutProgram;
using FluentValidation;

namespace Application.Validators.WorkoutProgram
{
    public class UpdateWorkoutProgramDtoValidator : AbstractValidator<UpdateWorkoutProgramDto>
    {
        public UpdateWorkoutProgramDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçerli bir program ID zorunludur.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık alanı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Başlık 100 karakteri geçemez.");

            RuleFor(x => x.Level)
                .NotEmpty().WithMessage("Program zorluk seviyesi zorunludur.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama alanı boş bırakılamaz.")
                .MaximumLength(500).WithMessage("Açıklama 500 karakteri geçemez.");

            RuleFor(x => x.Ambition)
                .NotEmpty().WithMessage("Hedef (Ambition) alanı zorunludur.");

            RuleFor(x => x.ProgramExercises)
                .NotEmpty().WithMessage("Programa en az bir egzersiz eklenmelidir.");
        }
    }
}
