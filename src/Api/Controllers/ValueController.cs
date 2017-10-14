using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Controllers
{
    public class ValueController : Controller, IValueServiceApi
    {
        public Task<IEnumerable<CustomValue>> GetAllValues()
        {
            throw new System.NotImplementedException();
        }
        
        public Task<IEnumerable<CustomValue>> GetValue(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task CreateValue(CustomValue value)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateValue(CustomValue value)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteValue(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
