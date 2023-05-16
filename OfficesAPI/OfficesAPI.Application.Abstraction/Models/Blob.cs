namespace OfficesAPI.Application.Abstraction.Models
{
    public record Blob(string FileName, string ContentType, byte[] Content);
}
