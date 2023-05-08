using FluentValidation.Results;
using Newtonsoft.Json.Linq;

namespace OfficesAPI.Application.Results
{
    public class ApplicationUpdateResult<T>:IApplicationResult
    {
        public T? OldValue { get; set; }
        public T? NewValue { get; set; }

        public IList<string> Errors {get;}

        public bool IsComplite => Errors.Count == 0;

        private ApplicationUpdateResult(T? oldValue, T? newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public ApplicationUpdateResult(T? oldValue = default, T? newValue = default, params string[] errors): this(oldValue, newValue)
        {
            Errors = errors.ToList();
        }

        public ApplicationUpdateResult(ValidationResult validationResult, T? oldValue = default, T? newValue = default) : this(oldValue, newValue)
        {
            Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
        }
    }
}
