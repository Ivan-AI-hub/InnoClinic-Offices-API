namespace OfficesAPI.Application.Abstraction.Models
{
    public record UpdateOfficeModel(PictureDTO? Photo,
                                    string City,
                                    string Street,
                                    int HouseNumber,
                                    int OfficeNumber,
                                    string PhoneNumber,
                                    bool Status);
}
