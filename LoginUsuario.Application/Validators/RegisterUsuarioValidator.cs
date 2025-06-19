using FluentValidation;
using LoginUsuario.Comunication.Requests;

namespace LoginUsuario.Application.Validators
{
    public class RegisterUsuarioValidator : AbstractValidator<RequestRegisterUsuarioJson>
    {
        public RegisterUsuarioValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MinimumLength(3).WithMessage("O nome deve conter no mínimo 3 caracteres.")
                .MaximumLength(100).WithMessage("O nome deve conter no máximo 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigátório.")
                .EmailAddress().WithMessage("O e-mail informado é inválido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.");

            When(x => string.IsNullOrEmpty(x.Password) == false, () =>
            {
                RuleFor(x => x.Password.Length)
                    .GreaterThanOrEqualTo(6).WithMessage("A senha deve conter mais do que 6 caracteres. ");
            });
        }
    }
}
