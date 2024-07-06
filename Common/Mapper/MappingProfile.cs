using AutoMapper;
using demoAPI.Model;
using demoAPI.Model.DS;
using demoAPI.Model.TodlistsDone;
using demoAPI.Model.Trip;
using demoAPI.Model.Kanbans;
using demoAPI.Model.DS.Shops;
using demoAPI.Model.DS.Accounts;

namespace demoAPI.Common.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DSAccountAddReq, DSAccount>();

            CreateMap<ShopDiaryAddReq, ShopDiary>();
            
            CreateMap<ShopAddReq, Shop>();
            CreateMap<Shop, ShopDto>();

            CreateMap<ShopTypeAddReq, ShopType>();
            CreateMap<ShopType, ShopTypeDto>();            

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

            CreateMap<MemberLoginReq, Member>();
            CreateMap<Member, MemberDto>();
        }
    }
}
