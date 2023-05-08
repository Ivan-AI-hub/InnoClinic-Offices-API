namespace OfficesAPI.Application.Results
{
    public interface IApplicationResult
    {
        public IList<string> Errors { get; }
        public bool IsComplite { get; }
    }
}
