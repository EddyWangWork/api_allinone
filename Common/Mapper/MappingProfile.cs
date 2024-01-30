using AutoMapper;
using demoAPI.Model;
using demoAPI.Model.DS;
using demoAPI.Model.TodlistsDone;
using demoAPI.Model.Trip;
using demoAPI.Model.Kanbans;

namespace demoAPI.Common.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DSTransaction, DSTransactionDto>();
            CreateMap<DSTransactionReq, DSTransaction>();

            CreateMap<TodolistAddReq, Todolist>();
            CreateMap<TodolistDoneAddReq, TodolistDone>();
            CreateMap<TodolistDoneEditReq, TodolistDone>();

            CreateMap<DSItemAddReq, DSItem>();
            CreateMap<DSItemSubAddReq, DSItemSub>();

            CreateMap<TripAddReq, Trip>();
            CreateMap<TripDetailTypeAddReq, TripDetailType>();
            CreateMap<TripDetailAddReq, TripDetail>();

            CreateMap<KanbanAddReq, Kanban>();
        }
    }
}
