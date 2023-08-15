namespace demoAPI.Model.Trip
{
    public class TripDetailDto
    {
        public List<TripDetailTypeDto> TripDetailTypesInfo { get; set; }

        public TripDetailDto()
        {
            TripDetailTypesInfo = new List<TripDetailTypeDto>();
        }
    }
}
