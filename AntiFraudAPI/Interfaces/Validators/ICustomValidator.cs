using Domain.Enums;

namespace Interfaces.Validators
{
    public interface ICustomValidator<T>
    {
        Task<bool> IsValidAsync(T t, ActionType actionType);
        List<string> Errors { get; }
    }
}
