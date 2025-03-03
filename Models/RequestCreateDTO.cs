namespace prsquest_api_controllers.Models
{
    public class RequestCreateDTO
    {
        public int UserId { get; set; } // place held for front end actions required
        public string Description { get; set; }
        public string Justification { get; set; }
        public DateOnly DateNeeded { get; set; }
        public string DeliveryMode { get; set; }
        // public string  RequestNumber { get; set; }
        // public string Status { get; set; }
        // public DateTime SubmittedDate { get; set; }
        // public decimal Total { get; set; }
    }
}
