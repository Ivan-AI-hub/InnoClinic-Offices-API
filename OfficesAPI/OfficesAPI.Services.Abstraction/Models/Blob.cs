namespace OfficesAPI.Services.Abstraction.Models
{
    public record Blob(string FileName, string ContentType, byte[] Content);
}
