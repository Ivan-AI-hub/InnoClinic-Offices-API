namespace OfficesAPI.Domain
{
    public class Picture
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Format { get; private set; }
        public string Description { get; private set; }
        public byte[] Image { get; private set; }

        private Picture() { }
        public Picture(string name, string format, string description, byte[] image)
        {
            Id = Guid.NewGuid();
            Name = name;
            Format = format;
            Description = description;
            Image = image;
        }
    }
}
