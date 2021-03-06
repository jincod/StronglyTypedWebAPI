using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace Models
{
    public interface IValueServiceRefitApi
    {
        [Get("/value")]
        Task<IEnumerable<CustomValue>> GetAllValues();

        [Get("/value/{id}")]
        Task<CustomValue> GetValue(string id);

        [Post("/value")]
        Task CreateValue(CustomValue value);

        [Put("/value/{id}")]
        Task UpdateValue(string id, CustomValue value);

        [Delete("/value/{id}")]
        Task DeleteValue(string id);
    }
}
