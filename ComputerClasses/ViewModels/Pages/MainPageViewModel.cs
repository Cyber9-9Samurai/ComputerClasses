using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ComputerClasses.Domain;
using ComputerClasses.Models;
using ComputerClasses.Services;
using ComputerClasses.Services.Data;
using ComputerClasses.ViewModels.Abstractions;
using ComputerClasses.ViewModels.Popups;
using Mvvm.Navigation;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WpfAnimatedGif;

namespace ComputerClasses.ViewModels.Pages
{
    //логика работы главной страницы
    public partial class MainPageViewModel : PageBaseViewModel
    {
        //отслеживаемое поле которое хранит сслыку 
        //на список кнопок управления данными
        [ObservableProperty]
        private List<MenuButtonItem> operationButtons = new();
        //отслеживаемое поле которое хранит значение true,
        //если файл импортировали
        [ObservableProperty]
        private bool isExistFile;
        //отслеживаемое поле которое хранит сслыку 
        //на коллекцию импортированных данных
        [ObservableProperty]
        private ObservableCollection<Row> rows = new();
        //отслеживаемое поле которое хранит сслыку
        //а строку которую ввели в поисковом поле
        [ObservableProperty]
        private string searchText;
        //отслеживаемое поле которое хранит сслыку
        //на выбранную строку из коллекции данных
        [ObservableProperty]
        private Row? selectedRow;

        //поле которое хранит сслыку 
        //на импортированную коллекцию для восстановления значений отображаемой
        private List<Row> baseRows = new();

        //поле которое хранит ссылку на навигатор для модальных окон
        private readonly Navigator<PopupBaseViewModel> _navigator;
        //поле которое хранит ссылку на сервис для получения изображений
        private readonly GetLocalImage _imageService;
        //поле которое хранит ссылку на сервис для хранения текущего открытого файла
        private readonly WorkFileService _workFileService;
        //поле которое хранит ссылку значение количесва компьютеров
        //по номеру корпуса и номера аудитории
        private Dictionary<(int, int), int> comps = new();

        private int lastSearchLenght = 0;

        //получение зависимостей через конструктор с помощью DI
        public MainPageViewModel(Navigator<PopupBaseViewModel> navigator,
            GetLocalImage imageService,
            WorkFileService workFileService)
        {
            //передача ссылок
            _navigator = navigator;
            _imageService = imageService;
            _workFileService = workFileService;
            //вызов метода для загрузки данных
            LoadData();
            
        }

        //метод загрузки данных
        private void LoadData()
        {
            //проверка первая ли это загрузка приложения
            if (!IsExistFile)
            {
                //создание кнопок и привязка к ним команд и изображений, 
                //а также передача ссылок на аниматор
                OperationButtons = new List<MenuButtonItem>()
                {
                    new MenuButtonItem("Добавить",_imageService.GetImage("Add.png"),AddCommand,null),
                    new MenuButtonItem("Редактировать",_imageService.GetImage("Edit.png"),EditCommand,null),
                    new MenuButtonItem("Удалить",_imageService.GetImage("Delete.png"),RemoveCommand,null),
                    new MenuButtonItem("Обновить",_imageService.GetImage("Sync.png"),LoadFileCommand,null)
                };
                //подписка на событие при изменении полей в 
                //сервисе для хранения рабочего файла
                _workFileService.PropertyChanged += async (s, e) =>
                {   
                    //проверка если изменился сам файл
                    if (_workFileService.fileChanged == e.PropertyName)
                    {

                        //обновляем статус
                        IsExistFile = _workFileService.HasFile();
                        //загружаем данные из файла
                        await LoadFile();
                        
                    }
                };
                //подписка на событие при изменении текущего объекта
                this.PropertyChanged += async (s, e) =>
                {
                    //проверка если изменилось значение в поле поиска
                    if (e.PropertyName == nameof(SearchText))
                    {
                        //вызываем метод поиска
                        await DoSearch();
                    }
                };

                
            }

            //подсчет количесва компьютеров в текущем фале и заполенения коллекции comps
            foreach (var item in baseRows)
                {
                    if (int.TryParse(item.Frame.Name, out int frame) && int.TryParse(item.AudienceNumber.Name, out int number))
                    {
                        if (comps.ContainsKey((frame, number)))
                        {
                            comps[(frame, number)] += 1;
                        }
                        else
                        {
                            comps.Add((frame, number), 1);
                        }
                    }
                }


        }

        //метод для открытия модального окна фильтрации
        [RelayCommand]
        private void OpenFilter()
        {
            //проверка импортирован ли файл
            if (IsExistFile)
            {
                //отчистка стека истории навигаций
                _navigator.BackStack.Clear();
                //открытие модального окна настройки фильтров
                _navigator.Navigate<FilterPopupViewModel>();
            }
        }

        //метод для применения фильтров
        public void ApplyFilter(string?[] comboboxfilters, string?[] textboxfilters, bool isCleared = false)
        {
            //проверка если фильтры были сброшены
            if (isCleared)
            {
                //восстановление изначальной коллекции
                Rows = [.. baseRows];
                return;
            }

            //применение фильтров с помощью LINQ-запроса
            var filtered = baseRows.Where(row => //берется каждая строка из коллекции
            {
                //цикл для прохождения по всем фильтрам из ComboBox-ов
                for (int j = 0; j < comboboxfilters.Length; j++)
                {
                    //если фильтр не выбран, не рассматриваем
                    if (string.IsNullOrEmpty(comboboxfilters[j])) continue;

                    //строка проверяется на соответствие j-ому фильтру
                    bool matches = j switch
                    {
                        0 => row.Status?.Name.Contains(comboboxfilters[j]) ?? false,
                        1 => row.OperatingSystem?.Name.Contains(comboboxfilters[j]) ?? false,
                        2 => row.ResponsiblePerson?.Name.Contains(comboboxfilters[j]) ?? false,
                        3 => row.Facultie?.Name.Contains(comboboxfilters[j]) ?? false,
                        4 => row.RamType?.Name.Contains(comboboxfilters[j]) ?? false,
                        5 => row.Frame?.Name.Contains(comboboxfilters[j]) ?? false,
                        _ => true
                    };
                    
                    //если строка не соответсвует хотя бы одному фильтру,
                    //то она нам не подходит
                    if (!matches) return false; 
                }

                //цикл для прохождения по всем фильрам из TextBox-ов
                for (int j = 0; j < textboxfilters.Length; j++)
                {
                    //если фильтр пустой, то не рассматриваем
                    if (string.IsNullOrEmpty(textboxfilters[j])) continue;

                    //строка проверяется на соответсвие j-ому фильтру
                    bool matches = j switch
                    {
                        0 => int.TryParse(row.Ram?.Name, out int ram) &&
                             ram > int.Parse(textboxfilters[j]),
                        1 => int.TryParse(row.Frame?.Name, out int frame) &&
                             int.TryParse(row.AudienceNumber?.Name, out int number) &&
                             comps.ContainsKey((frame, number)) &&
                             comps[(frame, number)] >= int.Parse(textboxfilters[j]),
                        _ => true
                    };

                    //если строка не соответсвует хотя бы одному фильтру,
                    //то она нам не подходит
                    if (!matches) return false;
                }

                //если все фильтры применены успешно, 
                //то элемент добавляется в итоговую коллекцию
                return true;
            }).ToList(); //перевод все коллекции в список

            //замена текущей коллекции на отфильтрованную
            Rows = [.. filtered]; 
        }



        //метод загрузки данных из файла
        [RelayCommand]
        public async Task LoadFile()
        {
            if (IsExistFile)
            {
                //передача ссылки на данные в коллекцию для восстановления значений
                baseRows = [.. _workFileService.ImportData.Items];
                //передачи сслыки на данные в отображаемую коллекцию
                Rows = _workFileService.ImportData.Items;
            }
        }

        //метод поиска по названию аудитории или установленным приложениям
        private async Task DoSearch()
        {
            if(lastSearchLenght > 0 && SearchText.Length-lastSearchLenght < 0)
            {
                await Task.Delay(700);
            }
            //нормализация значения
            lastSearchLenght = SearchText.Length;
            var text = SearchText.ToLower();
            //проверка если текст поиска пуст
            if (!string.IsNullOrWhiteSpace(text))
            {
                //обновлеям отображаемую коллекцию
                Rows = [.. baseRows.Where(i => i.AudienceName.Name.ToLower().Contains(text) 
                || i.ApplicationList.Name.ToLower().Contains(text))];
            }
            //проверка если коичество элементов в отображаемой коллекции
            //меньше чем должно быть
            else if (baseRows.Count > Rows.Count)
            {   
                //восстанавливаем значение
                Rows = [.. baseRows];
            }
        }

        //метод открытия модального окна взаимодействия с данными для добавления
        [RelayCommand]
        private void Add()
        {
            //проверка если файл существует
            if (IsExistFile)
            {
                //открытие модального окна и передача значений
                SelectedRow = null;
                _navigator.Navigate<ChangeDataPopupViewModel>().GetData(DataChangesActions.Add, SelectedRow);
            }
        }

        //метод открытия модального окна взаимодействия с данными для удаления
        [RelayCommand]
        private void Remove()
        {
            //проверка если выбрана строка
            if (SelectedRow != null)
            {
                //открытие модального окна и передача значений
                _navigator.Navigate<ChangeDataPopupViewModel>().GetData(DataChangesActions.Remove, SelectedRow);
            }
        }

        //метод открытия модального окна взаимодействия с данными для изменения
        [RelayCommand]
        private void Edit()
        {
            //проверка если выбрана строка
            if (SelectedRow != null)
            {
                //открытие модального окна и передача значений
                _navigator.Navigate<ChangeDataPopupViewModel>().GetData(DataChangesActions.Edit, SelectedRow);
            }
        }

        //метод для создания файла, доступен если нет импортированного
        [RelayCommand]
        private void CreateFile()
        {
            //вызов метода для создания файла
            _workFileService.CreateNewWorkFile();
        }

    }
}
