namespace prsquest_api_controllers.Models
{
    public class RequestStatus
    {
        public string NEW   { get; set; }
        public string REVIEW { get; set; }
        public string APPROVED { get; set; }
        public string REJECTED { get; set; }

        public RequestStatus ()
        {
            NEW = "NEW";
            REVIEW = "REVIEW";
            APPROVED = "APPROVED";
            REJECTED = "REJECTED";
        }
    }
}
