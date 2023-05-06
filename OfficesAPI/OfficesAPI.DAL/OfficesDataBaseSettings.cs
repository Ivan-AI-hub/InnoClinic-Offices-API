namespace OfficesAPI.DAL
{
    public class OfficesDataBaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string OfficesCollectionName { get; set; } = null!;
    }
}
