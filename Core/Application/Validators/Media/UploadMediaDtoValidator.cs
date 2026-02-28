using Application.DTO.Media;
using FluentValidation;

namespace Application.Validators.Media
{
    public class UploadMediaDtoValidator : AbstractValidator<UploadMediaDto>
    {
        public UploadMediaDtoValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("Geçersiz veya boş dosya yüklendi.")
                .Must(f => f != null && f.Length > 0).WithMessage("Geçersiz veya boş dosya yüklendi.");
        }
    }
}
