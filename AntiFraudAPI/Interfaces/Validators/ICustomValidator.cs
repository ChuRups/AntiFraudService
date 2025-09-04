using Domain.Enums;

namespace Interfaces.Validators
{
    public interface ICustomValidator<T>
    {
        Task<bool> IsValid(T t, ActionType actionType);
        List<string> Errors { get; }
    }
}
