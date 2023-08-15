using demoAPI.Model.Trip;

namespace demoAPI.BLL.Trips
{
    public interface ITripBLL
    {
        Task<IEnumerable<TripResultDto>> GetTrips();
        Task<Trip> AddTrip(TripAddReq req);
        Task<Trip> UpdateTrip(int id, TripAddReq req);
        Task<Trip> DeleteTrip(int id);

        Task<IEnumerable<TripDetailType>> GetTripDetailTypes();
        Task<TripDetailType> AddTripDetailType(TripDetailTypeAddReq req);
        Task<TripDetailType> UpdateTripDetailType(int id, TripDetailTypeAddReq req);
        Task<TripDetailType> DeleteTripDetailType(int id);

        Task<TripDetail> AddTripDetail(TripDetailAddReq req);
        Task<IEnumerable<TripDetail>> GetTripDetails();
        Task<TripDetail> UpdateTripDetail(int id, TripDetailAddReq req);
        Task<TripDetail> DeleteTripDetail(int id);
    }
}
