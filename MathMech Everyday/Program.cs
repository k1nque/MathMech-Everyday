using System;
using System.Net;
using System.Runtime.InteropServices;
using NCron;
using NCron.Fluent;
using NCron.Fluent.Crontab;
using NCron.Fluent.Generics;
using NCron.Service;
using Parser.TimerTasks;


namespace MathMech_Everyday
{
    class Program
    {
        public void Main()
        {

            var shedulingService = new SchedulingService();
            shedulingService.Daily().Run<TimerTasks>();
        }
    }

}