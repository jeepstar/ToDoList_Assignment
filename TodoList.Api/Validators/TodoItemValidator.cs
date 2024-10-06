using FluentValidation;
using TodoList.Api.Models;

namespace TodoList.Api.Validators
{
    public class TodoItemValidator : AbstractValidator<TodoItem>
    {
        public TodoItemValidator()
        {
            RuleFor(item => item.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(100).WithMessage("Description cannot exceed 100 characters.");
        }
    }
}
