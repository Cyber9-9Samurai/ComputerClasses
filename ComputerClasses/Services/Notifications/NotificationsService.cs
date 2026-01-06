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
                DateTime temp = lastUpdateDate.AddDays(settings.Day).AddMonths(settings.Month).AddYears(settings.Year);

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
            }, Formatting.Indented);

            string filePath = "notifications_settings.json";
            File.WriteAllText(filePath, jsonString);
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
