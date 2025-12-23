using ComputerClasses.Domain;
using ComputerClasses.Domain.Entities;
using Newtonsoft.Json;
using System.IO;

namespace ComputerClasses.Services.Notifications
{
    public class NotificationsService
    {
        public async Task<List<string>> NotificationsServiceStart(List<Row> rows)
        {
            NotificationsJson settings;
            if (File.Exists("notifications_settings.json"))
            {
                settings = await ReadJsonFile("notifications_settings.json");
            }
            else
            {
                await CreateJson();
                settings = await ReadJsonFile("notifications_settings.json");
            }


            DateTime currentDate = DateTime.Now;

            List<string> notifications = [];

            foreach (Row row in rows)
            {
                if (row.LastServiceDate.Name == "Нет данных")
                {
                    continue;
                }
                DateTime lastUpdateDate = DateTime.Parse(row.LastServiceDate.Name);
                DateTime temp = await Bbhdhsbcbd(settings, lastUpdateDate);

                if (temp <= currentDate)
                {
                    notifications.Add($"Компьютер {row.Id} требуется в обслуживании");
                }
            }
            return notifications;
        }

        public async Task CreateJson()
        {
            string jsonString = JsonConvert.SerializeObject(new NotificationsJson
            {
                Year = 5,
                Month = 0,
                Day = 0
            }, Newtonsoft.Json.Formatting.Indented);

            string filePath = "notifications_settings.json";
            File.WriteAllText(filePath, jsonString);
        }

        public async Task<DateTime> Bbhdhsbcbd(NotificationsJson dataString, DateTime lastUpdateDate)
        {
            return lastUpdateDate.AddDays(dataString.Day).AddMonths(dataString.Month).AddYears(dataString.Year);
        }

        public static async Task<NotificationsJson> ReadJsonFile(string path)
        {
            string jsonFromFile = File.ReadAllText("notifications_settings.json");
            NotificationsJson notificationsFromFile = JsonConvert.DeserializeObject<NotificationsJson>(jsonFromFile);
            return notificationsFromFile;
        }

        public async Task<DateTime> ConvertToDateTime(NotificationsJson notifications)
        {
            return new DateTime(notifications.Year, notifications.Month, notifications.Day);
        }

    }
}
