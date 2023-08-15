namespace demoAPI.Model.Trip
{
    public class TripDto
    {
        public DateTime Date { get; set; }
        public List<TripDetailDto> TripDetailDtos { get; set; }

        public TripDto()
        {
            TripDetailDtos = new List<TripDetailDto>();
        }
    }

    public class TripResultDto
    {
        public string Name { get; set; }
        public List<TripDto> TripDtos { get; set; }

        public TripResultDto()
        {
            TripDtos = new List<TripDto>();
        }
    }
}
