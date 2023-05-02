namespace OfficesAPI.Domain
{
    public class OfficeAddress
    {
        public string City { get; private set; }
        public string Street { get; private set; }
        public int HouseNumber { get; private set; }

        private OfficeAddress() { }
        public OfficeAddress(string city, string street, int houseNumber)
        {
            City = city;
            Street = street;
            HouseNumber = houseNumber;
        }
    }
}
