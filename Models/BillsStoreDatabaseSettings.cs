namespace BillsClientApi.Models
{
    public class BillsStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string BillsCollectionName { get; set; } = null!;
    }
}
