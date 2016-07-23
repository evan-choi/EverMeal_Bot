using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverMeal_Bot.Scheduler
{
    public class Schedule
    {
        public ScheduleOption Option { get; set; }
        public Action<object> Action { get; set; }
    }
}
