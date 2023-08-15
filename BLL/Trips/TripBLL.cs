using AutoMapper;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Model.Exceptions;
using demoAPI.Model.Trip;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demoAPI.BLL.Trips
{
    public class TripBLL : BaseBLL, ITripBLL
    {
        private readonly DSContext _context;
        private readonly IMapper _mapper;

        public TripBLL(DSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //--------------------TRIP ALL

        public async Task<IEnumerable<TripResultDto>> GetTrips()
        {
            var tripDetails = await (
                 from a in _context.TripDetails
                 join b in _context.TripDetailTypes on a.TripDetailTypeID equals b.ID
                 join c in _context.Trips on a.TripID equals c.ID
                 select new
                 {
                     TripName = c.Name,
                     a.Date,
                     TripDetailTypeName = b.Name,
                     TripDetailName = a.Name,
                     TripDetailNameLink = a.LinkName
                 }).ToListAsync();


            var tripResultsDto = new List<TripResultDto>();

            var tripsInfo = await _context.Trips.ToListAsync();
            var tripDetailTypes = await _context.TripDetailTypes.ToListAsync();

            var tripDistinctTypeNames = tripDetailTypes.Select(x => x.Name);
            var tripGDates = tripDetails.GroupBy(x => new { x.TripName, x.Date });

            foreach (var tripInfo in tripsInfo)
            {
                var tripsDto = new List<TripDto>();

                var diffDay = tripInfo.ToDate - tripInfo.FromDate;

                for (int i = 0; i < diffDay.Days + 1; i++)
                {
                    var tripGTypesDto = new List<TripDetailTypeDto>();
                    var tripDetailDtos = new List<TripDetailDto>();

                    var tripDate = tripInfo.FromDate.AddDays(i);
                    var tripGTypes = tripGDates.FirstOrDefault(x => x.Key.TripName == tripInfo.Name && x.Key.Date == tripDate)?.GroupBy(y => y.TripDetailTypeName);

                    foreach (var tripDistinctTypeName in tripDistinctTypeNames)
                    {
                        //var typeValues = tripGTypes?.FirstOrDefault(x => x.Key == tripDistinctTypeName)?.Select(x => x.TripDetailName)?.ToList();
                        var typeValues = tripGTypes?.FirstOrDefault(x => x.Key == tripDistinctTypeName)?.
                            Select(x => new TripDetailTypeValueDto
                            {
                                TypeValue = x.TripDetailName,
                                TypeVTypeLink = (x.TripDetailNameLink == null || x.TripDetailNameLink == string.Empty) ? string.Empty : x.TripDetailNameLink
                            })?
                            .ToList();

                        if (tripGTypes == null || typeValues == null)
                        {
                            typeValues = new List<TripDetailTypeValueDto> { new TripDetailTypeValueDto { TypeValue = "-", TypeVTypeLink = string.Empty } };
                        }

                        tripGTypesDto.Add(new TripDetailTypeDto
                        {
                            TypeName = tripDistinctTypeName,
                            TypeValues = typeValues
                        });


                        if (tripGTypesDto.Count() == 3)
                        {
                            tripDetailDtos.Add(new TripDetailDto { TripDetailTypesInfo = tripGTypesDto });
                            tripGTypesDto = new List<TripDetailTypeDto>();
                        }
                        else if (tripDistinctTypeNames.Last() == tripDistinctTypeName && tripGTypesDto.Count() != 3)
                        {
                            tripDetailDtos.Add(new TripDetailDto { TripDetailTypesInfo = tripGTypesDto });
                        }
                    }
                    tripsDto.Add(new TripDto
                    {
                        Date = tripDate,
                        TripDetailDtos = tripDetailDtos
                    });
                }

                tripResultsDto.Add(new TripResultDto
                {
                    Name = tripInfo.Name,
                    TripDtos = tripsDto
                });
            }

            return tripResultsDto;
        }

        public async Task<Trip> AddTrip(TripAddReq req)
        {
            var entity = _mapper.Map<Trip>(req);

            _context.Trips.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<Trip> UpdateTrip(int id, TripAddReq req)
        {
            var entity = await _context.Trips.FirstOrDefaultAsync(x => x.ID == id) ??
                throw new NotFoundException("trip Record not found");

            _mapper.Map(req, entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<Trip> DeleteTrip(int id)
        {
            var entity = await _context.Trips.FirstOrDefaultAsync(x => x.ID == id) ??
                throw new NotFoundException("trip Record not found");

            var deleted = entity;

            _context.Trips.Remove(entity);
            _context.SaveChanges();

            return deleted;
        }

        //--------------------TRIP ALL END

        //--------------------TRIP Detail Type

        public async Task<TripDetailType> AddTripDetailType(TripDetailTypeAddReq req)
        {
            var entity = _mapper.Map<TripDetailType>(req);

            _context.TripDetailTypes.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<IEnumerable<TripDetailType>> GetTripDetailTypes()
        {
            return await _context.TripDetailTypes.ToListAsync();
        }

        public async Task<TripDetailType> UpdateTripDetailType(int id, TripDetailTypeAddReq req)
        {
            var entity = await _context.TripDetailTypes.FirstOrDefaultAsync(x => x.ID == id) ??
                throw new NotFoundException("trip detail type Record not found");

            _mapper.Map(req, entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<TripDetailType> DeleteTripDetailType(int id)
        {
            var entity = await _context.TripDetailTypes.FirstOrDefaultAsync(x => x.ID == id) ??
                throw new NotFoundException("trip detail type Record not found");

            var deleted = entity;

            _context.TripDetailTypes.Remove(entity);
            _context.SaveChanges();

            return deleted;
        }

        //--------------------TRIP Detail Type END

        //--------------------TRIP Detail

        public async Task<TripDetail> AddTripDetail(TripDetailAddReq req)
        {
            var trip = await _context.Trips.FirstOrDefaultAsync(x => x.ID == req.TripID) ??
                throw new NotFoundException("trip Record not found");

            var tripdetailtype = await _context.TripDetailTypes.FirstOrDefaultAsync(x => x.ID == req.TripDetailTypeID) ??
                throw new NotFoundException("trip detail type Record not found");

            var entity = _mapper.Map<TripDetail>(req);

            _context.TripDetails.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<IEnumerable<TripDetail>> GetTripDetails()
        {
            return await _context.TripDetails.ToListAsync();
        }

        public async Task<TripDetail> UpdateTripDetail(int id, TripDetailAddReq req)
        {
            var entity = await _context.TripDetails.FirstOrDefaultAsync(x => x.ID == id) ??
                throw new NotFoundException("trip detail Record not found");

            _mapper.Map(req, entity);
            _context.SaveChanges();

            return entity;
        }

        public async Task<TripDetail> DeleteTripDetail(int id)
        {
            var entity = await _context.TripDetails.FirstOrDefaultAsync(x => x.ID == id) ??
                throw new NotFoundException("trip detail Record not found");

            var deleted = entity;

            _context.TripDetails.Remove(entity);
            _context.SaveChanges();

            return deleted;
        }

        //--------------------TRIP Detail END
    }
}
