namespace demoAPI.Model.Trip
{
    public class TripDto
    {
        public DateTime Date { get; set; }
        public TripDetailDto TripDetailDto { get; set; }
    }

    public class TripResultDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<TripDto> TripDtos { get; set; }

        public TripResultDto()
        {
            TripDtos = new List<TripDto>();
        }
    }
}
