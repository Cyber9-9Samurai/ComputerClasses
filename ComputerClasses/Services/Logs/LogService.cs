using ComputerClasses.Services.Data;
using System.IO;
using System.Security.Cryptography;

namespace ComputerClasses.Services.Logs
{
    public class LogService
    {
        public async Task<List<string>> GetLog(Stream streamExcel)
        {
            var hashExcel = await HashFile(streamExcel);
            List<string> logs = [];
            if (Directory.Exists("logs") && File.Exists($"logs/{hashExcel}.txt"))
            {
                logs = File.ReadAllLines($"logs/{hashExcel}.txt").ToList();
            }
            return logs;
        }

        public async Task<List<string>> WriteLogLine(List<string> logs, DataChangesActions actions, ComputerClasses.Domain.Row addOrEditRow, string name)
        {
            string row = $"{addOrEditRow.Id} " +
                $"{addOrEditRow.Facultie.Name} " +
                $"{addOrEditRow.DepartmentOrInstitute.Name} " +
                $"{addOrEditRow.Frame.Name} " +
                $"{addOrEditRow.AudienceNumber.Name} " +
                $"{addOrEditRow.AudienceName.Name} " +
                $"{addOrEditRow.ResponsiblePerson.Name} " +
                $"{addOrEditRow.InventoryNumber.Name} " +
                $"{addOrEditRow.OperatingSystem.Name} " +
                $"{addOrEditRow.Motherboard.Name} " +
                $"{addOrEditRow.Cpu.Name} " +
                $"{addOrEditRow.VideoCard.Name} " +
                $"{addOrEditRow.Disk.Name} " +
                $"{addOrEditRow.Ram.Name} " +
                $"{addOrEditRow.RamType.Name} " +
                $"{addOrEditRow.ApplicationList.Name} " +
                $"{addOrEditRow.Status.Name} " +
                $"{addOrEditRow.LastServiceDate.Name}";
            switch (actions)
            {
                case DataChangesActions.Add:
                    logs.Add(await WriteLog($"Добавлен компютер: {row} ", name));
                    break;
                case DataChangesActions.Edit:
                    logs.Add(await WriteLog($"Изменён компьютер {addOrEditRow.Id}: {row}", name));
                    break;
                case DataChangesActions.Remove:
                    logs.Add(await WriteLog($"Удалён компьютер {addOrEditRow}", name));
                    break;
            }
            return logs;
        }

        private static async Task<string> WriteLog(string message, string name)
        {
            string logEntry = $"[{DateTime.Now}][{name}]\t{message}";
            return logEntry;
        }

        public async Task<string> HashFile(Stream stream)
        {
            using var sha256 = SHA256.Create();

            byte[] hash = await sha256.ComputeHashAsync(stream);
            return Convert.ToHexString(hash);
        }

        public async Task SaveLog(string hashExcel, List<string> logs)
        {
            if (!Directory.Exists("logs"))
            {
                Directory.CreateDirectory("logs");
            }

            string logstring = "";
            foreach (var log in logs)
            {
                logstring += log + "\n";
            }

            string filePath = $"logs/{hashExcel}.txt";
            File.WriteAllText(filePath, logstring);
        }
    }
}
