using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EverMeal_Bot.Scheduler;
using System.Threading;

namespace EverMeal_Bot
{
    class Program
    {

        static void Main(string[] args)
        {
            var s = new Schedule()
            {
                Action = heroku_wakeUp,
                Option = new ScheduleOption()
                {
                    EventType = EventType.Delay,
                    Delay = 1000 * 60 * 5,
                    Count = -1
                }
            };

            var u = new Schedule()
            {
                Action = heroku_updateAll,
                Option = new ScheduleOption()
                {
                    EventType = EventType.Alarm,
                    Count = -1,
                    Date = new TimeSpan(7, 0 ,0)
                }
            };

            Scheduler.Scheduler.Push(s);
            Scheduler.Scheduler.Push(u);

            Scheduler.Scheduler.Begin();
        }

        static async void heroku_updateAll(object obj)
        {
            if (obj == null)
                return;

            DateTime dt = (DateTime)obj;
            var result = await Heroku.Heroku.UpdateAll(dt.Year, dt.Month, dt.Day);

            var cc = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"{DateTime.Now:[hh:mm:ss]} Heroku> meal update - " + (result ? "success" : "failed"));

            Console.ForegroundColor = cc;
        }

        static async void heroku_wakeUp(object obj)
        {
            try
            {
                var result = await Heroku.Heroku.WakeUp();

                Console.WriteLine($"{DateTime.Now:[hh:mm:ss]} Heroku> wake up - " + (result ? "success" : "failed"));
            }
            catch
            {
                Console.WriteLine("Error - wake up");
            }
        }
    }
}