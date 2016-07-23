using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EverMeal_Bot.Heroku
{
    public static class Heroku
    {
        private const string UpdateKey = "6041cef9600a531f527a69186b66bd21";

#if DEBUG
        private static string targetUrl = "http://127.0.0.1:8080/";
#else
        private static string targetUrl = "http://evermeal.herokuapp.com/";
#endif

        private static HttpHelper helper;

        static Heroku()
        {
            helper = new HttpHelper(true);
        }

        public static async Task<bool> WakeUp()
        {
            var res = await helper.GET(targetUrl);

            return res.Item2 == HttpStatusCode.OK;
        }

        public static async Task<bool> UpdateAll(int year, int month, int day)
        {
            string url = $"{targetUrl}/feed/update?key={UpdateKey}&year={year}&month={month}&day={day}";

            return (await helper.GET(url)).Item2 == HttpStatusCode.OK;
        }
    }
}
