using Microsoft.AspNetCore.Http;

namespace OfficesAPI.Services.Abstraction.Models
{
    public record CreateOfficeModel(IFormFile? Photo,
                                    string City,
                                    string Street,
                                    int HouseNumber,
                                    int OfficeNumber,
                                    string PhoneNumber,
                                    bool Status);
}
