using AutoMapper;
using demoAPI.Model;
using demoAPI.Model.DS;
using demoAPI.Model.TodlistsDone;

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
        }
    }
}
