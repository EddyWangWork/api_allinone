namespace demoAPI.Model.Trip
{
    public class TripDetailTypeDto
    {
        public string TypeName { get; set; }
        public List<TripDetailTypeValueDto> TypeValues { get; set; }

        public TripDetailTypeDto()
        {
            TypeValues = new List<TripDetailTypeValueDto>();
        }
    }

    public class TripDetailTypeValueDto
    {
        public string TypeValue { get; set; }
        public string TypeVTypeLink { get; set; }
    }
}
