using Application.DTO.Community.Comment;
using FluentValidation;

namespace Application.Validators.Comment
{
    public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentDtoValidator()
        {
            RuleFor(x => x.PostId)
                .GreaterThan(0).WithMessage("Geçerli bir post ID'si giriniz.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Yorum içeriği boş olamaz.")
                .MaximumLength(1000).WithMessage("Yorum içeriği 1000 karakteri geçemez.");

            RuleFor(x => x.ParentCommentId)
                .GreaterThan(0).WithMessage("Geçerli bir yorum ID'si giriniz.")
                .When(x => x.ParentCommentId.HasValue);
        }
    }
}
