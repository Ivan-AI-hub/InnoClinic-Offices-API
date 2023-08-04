namespace OfficesAPI.Application.Abstraction.Models
{
    public class PictureDTO
    {
        public string Name { get; private set; }

        public PictureDTO(string name)
        {
            Name = name;
        }
    }
}
