﻿using System;
using System.Net;

namespace MathMech_Everyday
{
    class Program
    {
        static void Main(string[] args)
        {
            GetRequest.DownloadCalendar();
        }
    }

    public static class GetRequest
    {
        public static void DownloadCalendar()
        {
            using (var client = new WebClient())
            {
                client.DownloadFile("https://urfu.ru/api/schedule/groups/calendar/986697/20211107/", "calendar.ics");
            }
        }
    }
}