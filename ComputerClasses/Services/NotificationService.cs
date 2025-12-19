using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerClasses.Services
{
	/// <summary>
	/// Сервис уведомлений
	/// </summary>
	public class NotificationService
	{
		/// <summary>
		/// Показывает сообщение пользователю
		/// </summary>
		/// <param name="message">Текст сообщения</param>
		public void ShowInfo(string message)
		{
			MessageBox.Show(message, "Информация", 
				MessageBoxButton.OK, MessageBoxImage.Information);
		}

		/// <summary>
		/// Показывает сообщение об ошибке пользователю
		/// </summary>
		/// <param name="message">Текст сообщения об ошибке</param>
		public void ShowError(string message)
		{
			MessageBox.Show(message, "Ошибка",
				MessageBoxButton.OK, MessageBoxImage.Error);
		}

		/// <summary>
		/// Показывает сообщение подтверждение пользователю
		/// </summary>
		/// <param name="message">Текст сообщения подтверждения</param>
		public bool ShowConfirmation(string message)
		{
			var result = MessageBox.Show(message, "Подтверждение",
				MessageBoxButton.YesNo, MessageBoxImage.Question);

			return result == MessageBoxResult.Yes;
		}
	}
}
