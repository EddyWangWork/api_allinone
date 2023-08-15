namespace demoAPI.Model.Trip
{
    public class TripDetailAddReq
    {
        public int TripID { get; set; }
        public int TripDetailTypeID { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string? LinkName { get; set; }
    }
}
