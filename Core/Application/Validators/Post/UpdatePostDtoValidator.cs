using Application.DTO.Community.Post;
using FluentValidation;

namespace Application.Validators.Post
{
    public class UpdatePostDtoValidator : AbstractValidator<UpdatePostDto>
    {
        public UpdatePostDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçerli bir post ID'si giriniz.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Post içeriği boş olamaz.")
                .MaximumLength(2000).WithMessage("Post içeriği 2000 karakteri geçemez.");

            RuleFor(x => x.Visibility)
                .IsInEnum().WithMessage("Geçersiz görünürlük tipi.");

            RuleFor(x => x.MediaFiles)
                .Must(files => files == null || files.Count <= 10)
                .WithMessage("En fazla 10 medya dosyası yüklenebilir.");

            RuleForEach(x => x.MediaFiles)
                .Must(file => file.Length <= 50 * 1024 * 1024)
                .WithMessage("Her dosya en fazla 50 MB olabilir.")
                .When(x => x.MediaFiles != null && x.MediaFiles.Count > 0);
        }
    }
}
