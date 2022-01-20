using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ical.Net;
using static System.Globalization.CultureInfo;

namespace Parser
{
    public class Downloader
    {
        private int retries = 15;
        private readonly string pathToEmptyCal =
            Path.GetFullPath("empty.ics");
        
        public async Task<byte[]> DownloadFile(string url)
        {
            using var client = new WebClient();
            try
            {
                return await client.DownloadDataTaskAsync(url);
            }
            catch (WebException)
            {
                if (retries == 0)
                    return await File.ReadAllBytesAsync(pathToEmptyCal);
                retries -= 1;
                return await DownloadFile(url);
            }
        }
    }

    public class CalendarCreator
    {
        private const string Url = "https://urfu.ru/api/schedule/groups/calendar/";

        public async Task<Calendar> GetCalendar(string id, DateTime creationTime)
        {
            var lastSundayDate = creationTime.GetLastSunday();
            var dt = lastSundayDate.ToString("yyyyMMdd", InvariantCulture);
            var bytesContent = await new Downloader().DownloadFile(Url + id + "/" + dt);
            var content = Encoding.UTF8.GetString(bytesContent);
            return Calendar.Load(content);
        }
    }
}