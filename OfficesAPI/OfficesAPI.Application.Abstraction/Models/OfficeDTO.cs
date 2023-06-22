namespace OfficesAPI.Application.Abstraction.Models
{
    public class OfficeDTO
    {
        public Guid Id { get; set; }
        public PictureDTO? Photo { get; set; }
        public OfficeAddressDTO Address { get; set; }
        public int OfficeNumber { get; set; }
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }
    }
}
