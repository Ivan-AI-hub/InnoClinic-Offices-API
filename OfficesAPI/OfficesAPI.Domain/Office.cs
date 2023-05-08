namespace OfficesAPI.Domain
{
    public class Office
    {
        public Guid Id { get; private set; }
        public Picture? Photo { get; private set; }
        public OfficeAddress Address { get; private set; }
        public int OfficeNumber { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool Status { get; set; }

        private Office() { }
        public Office(OfficeAddress address, int officeNumber, string phoneNumber, bool status, Picture? photo)
        {
            Id = Guid.NewGuid();
            Address = address;
            OfficeNumber = officeNumber;
            PhoneNumber = phoneNumber;
            Status = status;
            Photo = photo;
        }
    }
}
