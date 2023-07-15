using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace demoAPI.Model.TodlistsDone
{
    public class TodolistDone
    {
        public int ID { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? Remark { get; set; }

        public int TodolistID { get; set; }
        public Todolist Todolist { get; set; }
    }
}
