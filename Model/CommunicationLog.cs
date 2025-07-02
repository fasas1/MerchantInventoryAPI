namespace MechantInventory.Model
{
    public class CommunicationLog
    {
        public Guid CommunicationLogId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Summary { get; set; }

        //Navigation
        public Customer Customer { get; set; }

    }
}
