using System.Threading.Tasks;
using Models;
using Refit;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var valueServiceApi = RestService.For<IValueServiceApi>("http://localhost:5000/api");

            Task.Run(async () =>
            {
                await valueServiceApi.GetAllValues();
                await valueServiceApi.GetValue("id1");
                await valueServiceApi.CreateValue(null);
                await valueServiceApi.UpdateValue("id1", null);
                await valueServiceApi.DeleteValue("id1");
            }).GetAwaiter().GetResult();
        }
    }
}
