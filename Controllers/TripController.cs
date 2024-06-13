using AutoMapper;
using demoAPI.BLL.Member;
using demoAPI.BLL.Trips;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Middleware;
using demoAPI.Model;
using demoAPI.Model.Trip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace demoAPI.Controllers
{
    [ApiController]
    [ResponseCompressionAttribute]
    [Route("[controller]")]
    public class TripController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITripBLL _tripBLL;

        public TripController(IMapper mapper, ITripBLL tripBLL)
        {
            _mapper = mapper;
            _tripBLL = tripBLL;
        }

        #region TRIP

        [HttpGet]
        [Route("getTrips")]
        public async Task<IActionResult> GetTrips()
        {
            var response = await _tripBLL.GetTrips();
            return Ok(response);
        }

        [HttpPost]
        [Route("addTrip")]
        public async Task<IActionResult> AddTrip(TripAddReq req)
        {
            var response = await _tripBLL.AddTrip(req);
            return Ok(response);
        }

        //[Route("updateTrip")]
        [HttpPut("updateTrip/{id}")]
        public async Task<IActionResult> UpdateTrip(int id, TripAddReq req)
        {
            var response = await _tripBLL.UpdateTrip(id, req);
            return Ok(response);
        }

        [HttpDelete("deleteTrip/{id}")]
        //[Route("deleteTrip")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var response = await _tripBLL.DeleteTrip(id);
            return Ok(response);
        }

        #endregion

        #region TRIP DETAIL TYPE

        [HttpGet]
        [Route("getTripDetailTypes")]
        public async Task<IActionResult> GetTripDetailTypes()
        {
            var response = await _tripBLL.GetTripDetailTypes();
            return Ok(response);
        }

        [HttpPost]
        [Route("addTripDetailType")]
        public async Task<IActionResult> AddTripDetailType(TripDetailTypeAddReq req)
        {
            var response = await _tripBLL.AddTripDetailType(req);
            return Ok(response);
        }

        //[Route("updateTripDetailType")]
        [HttpPut("updateTripDetailType/{id}")]
        public async Task<IActionResult> UpdateTripDetailType(int id, TripDetailTypeAddReq req)
        {
            var response = await _tripBLL.UpdateTripDetailType(id, req);
            return Ok(response);
        }

        [HttpDelete("deleteTripDetailType/{id}")]
        //[Route("deleteTripDetailType")]
        public async Task<IActionResult> DeleteTripDetailType(int id)
        {
            var response = await _tripBLL.DeleteTripDetailType(id);
            return Ok(response);
        }

        #endregion

        #region TRIP DETAIL

        [HttpGet]
        [Route("getTripDetails")]
        public async Task<IActionResult> GetTripDetails()
        {
            var response = await _tripBLL.GetTripDetails();
            return Ok(response);
        }

        [HttpPost]
        [Route("addTripDetail")]
        public async Task<IActionResult> AddTripDetail(TripDetailAddReq req)
        {
            var response = await _tripBLL.AddTripDetail(req);
            return Ok(response);
        }

        //[Route("updateTripDetail")]
        [HttpPut("updateTripDetail/{id}")]
        public async Task<IActionResult> UpdateTripDetail(int id, TripDetailAddReq req)
        {
            var response = await _tripBLL.UpdateTripDetail(id, req);
            return Ok(response);
        }

        [HttpDelete("deleteTripDetail/{id}")]
        //[Route("deleteTripDetail")]
        public async Task<IActionResult> DeleteTripDetail(int id)
        {
            var response = await _tripBLL.DeleteTripDetail(id);
            return Ok(response);
        }

        #endregion
    }
}
