namespace prsquest_api_controllers.Models
{
    public class RequestCreateDTO
    {
        public string Description { get; set; }
        public string Justification { get; set; }
        public DateOnly DateNeeded { get; set; }
        public string DeliveryMode { get; set; }
    }
}
