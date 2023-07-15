using demoAPI.Common.Enum;
using demoAPI.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace demoAPI.Model
{
    public class TodolistAddReq
    {
        [Required(ErrorMessage = "You should fill out a Name.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [EnumDataType(typeof(EnumTodolistType), ErrorMessage = "CategoryId should correct")]
        public int CategoryId { get; set; }
    }
}