using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerClasses.Services
{
	/// <summary>
	/// Сервис работы с логами
	/// </summary>
	public class ChangesLogService
	{
		/// <summary>
		/// Путь к логам
		/// </summary>
		private readonly string _logFilePath;

		public ChangesLogService()
		{
			_logFilePath = Path.Combine(
				AppDomain.CurrentDomain.BaseDirectory, 
				"changes.log");
		}

		/// <summary>
		/// Создать лог
		/// </summary>
		/// <param name="message">Текст лога</param>
		public void Log(string message)
		{
			var record = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}";

			File.AppendAllText(_logFilePath, record + Environment.NewLine);
		}
	}
}
