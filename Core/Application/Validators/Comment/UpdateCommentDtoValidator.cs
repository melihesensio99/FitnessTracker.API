using Application.DTO.Community.Comment;
using FluentValidation;

namespace Application.Validators.Comment
{
    public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
    {
        public UpdateCommentDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçerli bir yorum ID'si giriniz.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Yorum içeriği boş olamaz.")
                .MaximumLength(1000).WithMessage("Yorum içeriği 1000 karakteri geçemez.");
        }
    }
}
