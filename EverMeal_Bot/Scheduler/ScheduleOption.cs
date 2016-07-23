using System;

namespace EverMeal_Bot.Scheduler
{
    public class ScheduleOption
    {
        public EventType EventType { get; set; }

        public int Count { get; set; }

        public int Delay { get; set; }

        public TimeSpan Date { get; set; }

        public bool InitInvoke { get; set; } = false;
    }
}
