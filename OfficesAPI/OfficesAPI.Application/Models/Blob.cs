namespace OfficesAPI.Application.Models
{
    public record Blob(string FileName, string ContentType, Stream Content);
}
