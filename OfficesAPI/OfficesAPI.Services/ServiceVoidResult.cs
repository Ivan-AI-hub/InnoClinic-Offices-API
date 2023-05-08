using OfficesAPI.Application.Results;

namespace OfficesAPI.Services
{
    public class ServiceVoidResult
    {
        public IList<string> Errors { get; }
        public bool IsComplite => Errors.Count == 0;

        public ServiceVoidResult(params string[] errors)
        {
            Errors = errors.ToList();
        }

        public ServiceVoidResult(IApplicationResult applicationResult)
        {
            Errors = applicationResult.Errors;
        }
    }
}
