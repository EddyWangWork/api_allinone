using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using demoAPI.Common.Helper;

namespace demoAPI.Model.TodlistsDone
{
    public class TodolistDoneEditReq
    {
        private static double _unixUpdateTime;

        public double UnixUpdateTime
        {
            get => _unixUpdateTime;
            set => _unixUpdateTime = value;
        }

        public DateTime UpdateDate => DateTimeHelper.UnixToDateTimeMSec(_unixUpdateTime);

        public string? Remark { get; set; }
    }
}
