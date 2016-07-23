using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EverMeal_Bot.Scheduler
{
    public static class Scheduler
    {
        class ScheduleWrapper
        {
            public DateTime RegisteredTime { get; set; }
            public DateTime CurrentTime { get; set; }

            public Schedule Schedule { get; set; }
            public Thread Context { get; set; }

            public int Count { get; set; }
        }

        private static Queue<ScheduleWrapper> removeQueue;
        private static List<ScheduleWrapper> schedules;

        static Scheduler()
        {
            removeQueue = new Queue<ScheduleWrapper>();
            schedules = new List<ScheduleWrapper>();
        }

        public static void Push(Schedule schedule)
        {
            if (schedules.Where(s => s.Schedule.Equals(schedule)).Count() == 0)
            {
                schedules.Add(new ScheduleWrapper()
                {
                    RegisteredTime = DateTime.Now,
                    CurrentTime = DateTime.Now,
                    Schedule = schedule,
                    Context = Thread.CurrentThread
                });
            }
        }

        public static void Begin()
        {
            Worker();
        }

        private static void Worker()
        {
            foreach (ScheduleWrapper w in schedules)
                Invoke(w, null);

            var mre = new ManualResetEvent(false);

            while (schedules.Count > 0)
            {
                while (removeQueue.Count > 0)
                    schedules.Remove(removeQueue.Dequeue());

                foreach (ScheduleWrapper w in schedules)
                    Process(w);

                Thread.Sleep(500);
            }
        }

        private static void Process(ScheduleWrapper sw)
        {
            Schedule s = sw.Schedule;

            if (s.Option.EventType == EventType.Delay)
            {
                TimeSpan dt = DateTime.Now - sw.CurrentTime;

                if (dt.TotalMilliseconds >= s.Option.Delay)
                {
                    Invoke(sw, null);
                    sw.CurrentTime = sw.CurrentTime.AddMilliseconds(s.Option.Delay);

                    if (s.Option.Count > -1 && --s.Option.Count <= 0)
                    {
                        removeQueue.Enqueue(sw);
                    }
                }
            }
            else
            {
                var n = DateTime.Now;
                var vdt = new DateTime(n.Year, n.Month, n.Day,
                    sw.Schedule.Option.Date.Hours,
                    sw.Schedule.Option.Date.Minutes,
                    sw.Schedule.Option.Date.Seconds);

                var dt = n - vdt;

                if (dt.TotalMilliseconds >= 0 && dt.TotalMilliseconds < 500)
                {
                    Invoke(sw, vdt);
                }
            }
        }

        private static void Invoke(ScheduleWrapper sw, object obj)
        {
            sw.Schedule.Action.Invoke(obj);
        }

        private static AsyncCallback callback = new AsyncCallback((IAsyncResult result) =>
        {
            var sw = (ScheduleWrapper)result.AsyncState;
        });
    }
}