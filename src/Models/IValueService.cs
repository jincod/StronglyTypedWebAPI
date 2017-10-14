using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace Models
{
    public interface IValueServiceApi
    {
        [Get("value")]
        Task<IEnumerable<CustomValue>> GetAllValues();

        [Get("value/{id}")]
        Task<IEnumerable<CustomValue>> GetValue(string id);

        [Post("value")]
        Task CreateValue(CustomValue value);

        [Put("value/{id}")]
        Task UpdateValue(CustomValue value);

        [Delete("value/{id}")]
        Task DeleteValue(string id);
    }
}
