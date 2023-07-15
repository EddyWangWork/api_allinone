using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using demoAPI.Common.Helper;

namespace demoAPI.Model.TodlistsDone
{
    public class TodolistDoneAddReq : TodolistDoneEditReq
    {
        public int TodolistID { get; set; }
    }
}
