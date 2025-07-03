namespace MechantInventory.Model.Dto
{
    public class CommunicationCreateLogDto
    {
        public Guid CustomerId { get; set; }
        public string Type { get; set; }
        public string Summary { get; set; }
    }
}
