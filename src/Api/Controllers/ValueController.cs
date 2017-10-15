using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Controllers
{
    public class ValueController : Controller, IValueServiceApi
    {
        public Task<IEnumerable<CustomValue>> GetAllValues()
        {
            return Task.FromResult(new List<CustomValue>().AsEnumerable());
        }
        
        public Task<CustomValue> GetValue(string id)
        {
            return Task.FromResult(new CustomValue());
        }

        public Task CreateValue(CustomValue value)
        {
            return Task.FromResult(Ok());
        }

        public Task UpdateValue(string id, CustomValue value)
        {
            return Task.FromResult(Ok());
        }

        public Task DeleteValue(string id)
        {
            return Task.FromResult(Ok());
        }
    }
}
