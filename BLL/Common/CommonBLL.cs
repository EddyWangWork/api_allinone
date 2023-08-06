using AutoMapper;
using demoAPI.BLL.Common;
using demoAPI.BLL.DSItems;
using demoAPI.Common.Enum;
using demoAPI.Common.Helper;
using demoAPI.Data.DS;
using demoAPI.Model;
using demoAPI.Model.DS;
using demoAPI.Model.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace demoAPI.BLL.Common
{
    public class CommonBLL : BaseBLL, ICommonBLL
    {
        public CommonBLL()
        {
        }

        public async Task<object> GetDSTransTypes()
        {
            var response = new List<object>();

            foreach (var item in Enum.GetValues<EnumDSTranType>())
            {
                response.Add(new
                {
                    ID = (int)item,
                    Name = item.ToString()
                });
            }
            return response;
        }

        public async Task<object> GetTodolistTypes()
        {
            var response = new List<object>();

            foreach (var item in Enum.GetValues<EnumTodolistType>())
            {
                response.Add(new
                {
                    ID = (int)item,
                    Name = item.ToString()
                });
            }
            return response;
        }
    }
}
