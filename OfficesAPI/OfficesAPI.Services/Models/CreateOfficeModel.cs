using Microsoft.AspNetCore.Http;

namespace OfficesAPI.Services.Models
{
    public record CreateOfficeModel(IFormFile? Photo,
                                    string City,
                                    string Street,
                                    int HouseNumber,
                                    int OfficeNumber,
                                    string PhoneNumber,
                                    bool Status);
}
