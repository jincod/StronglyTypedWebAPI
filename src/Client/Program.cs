using System.Threading.Tasks;
using Models;
using Refit;

namespace Client
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var valueServiceApi = RestService.For<IValueServiceRefitApi>("http://localhost:5000/api");

            await valueServiceApi.GetAllValues();
            await valueServiceApi.GetValue("id1");
            await valueServiceApi.CreateValue(null);
            await valueServiceApi.UpdateValue("id1", null);
            await valueServiceApi.DeleteValue("id1");
        }
    }
}
