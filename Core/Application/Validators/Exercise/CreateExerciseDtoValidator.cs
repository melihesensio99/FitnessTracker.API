using Application.DTO.Exercise;
using FluentValidation;

namespace Application.Validators.Exercise
{
    public class CreateExerciseDtoValidator : AbstractValidator<CreateExerciseDto>
    {
        public CreateExerciseDtoValidator()
        {
            RuleFor(x => x.ExerciseName)
                .NotEmpty().WithMessage("Egzersiz adı boş olamaz.")
                .MaximumLength(100).WithMessage("Egzersiz adı 100 karakteri geçemez.");

            RuleFor(x => x.TargetMuscle)
                .NotEmpty().WithMessage("Hedef kas grubu zorunludur.")
                .MaximumLength(50).WithMessage("Hedef kas grubu 50 karakteri geçemez.");

            RuleFor(x => x.VideoUrl)
                .MaximumLength(500).WithMessage("Video URL 500 karakteri geçemez.")
                .When(x => !string.IsNullOrEmpty(x.VideoUrl));

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500).WithMessage("Resim URL 500 karakteri geçemez.")
                .When(x => !string.IsNullOrEmpty(x.ImageUrl));
        }
    }
}
