namespace OfficesAPI.Domain
{
    public class Picture
    {
        public string Name { get; private set; }

        private Picture() { }
        public Picture(string name)
        {
            Name = name;
        }
    }
}
