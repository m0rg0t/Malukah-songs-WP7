using Callisto.Controls;
using MalukahSongs.Data;
using MalukahSongs.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента страницы сгруппированных элементов задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234231

namespace MalukahSongs
{
    /// <summary>
    /// Страница, на которой отображается сгруппированная коллекция элементов.
    /// </summary>
    public sealed partial class GroupedItemsPage : MalukahSongs.Common.LayoutAwarePage
    {
        public GroupedItemsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Заполняет страницу содержимым, передаваемым в процессе навигации. Также предоставляется любое сохраненное состояние
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="navigationParameter">Значение параметра, передаваемое
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы.
        /// </param>
        /// <param name="pageState">Словарь состояния, сохраненного данной страницей в ходе предыдущего
        /// сеанса. Это значение будет равно NULL при первом посещении страницы.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Создание соответствующей модели данных для области проблемы, чтобы заменить пример данных
            var sampleDataGroups = SampleDataSource.GetGroups((String)navigationParameter);
            this.DefaultViewModel["Groups"] = sampleDataGroups;
        }

        /// <summary>
        /// Вызывается при нажатии заголовка группы.
        /// </summary>
        /// <param name="sender">Объект Button, используемый в качестве заголовка выбранной группы.</param>
        /// <param name="e">Данные о событии, описывающие, каким образом было инициировано нажатие.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Определение группы, представляемой экземпляром Button
            var group = (sender as FrameworkElement).DataContext;

            // Переход к соответствующей странице назначения и настройка новой страницы
            // путем передачи необходимой информации в виде параметра навигации
            this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)group).UniqueId);
        }

        /// <summary>
        /// Вызывается при нажатии элемента внутри группы.
        /// </summary>
        /// <param name="sender">Объект GridView (или ListView, если приложение прикреплено),
        /// в котором отображается нажатый элемент.</param>
        /// <param name="e">Данные о событии, описывающие нажатый элемент.</param>
        async void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.BottomAppBar.Visibility = Visibility.Visible;
            
            try
            {
                Player.Stop();

                ItemViewModel item = (ItemViewModel)e.ClickedItem;
                Player.Source = new Uri(item.Download_url);
                Player.Play();
            }
            catch { };
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                try
                {
                    App.ViewModel.LoadData();
                    App.ViewModel.DataLoad += new MainViewModel.DataLoadEventHandler(this.DataLoaded);
                }
                catch { };
            };
        }


        public static async Task<StorageFile> GetFileIfExistsAsync(StorageFolder folder, string fileName)
        {
            try
            {
                return await folder.GetFileAsync(fileName);
            }
            catch
            {
                return null;
            }
        }

        private async void SaveAsMusicInMusicLibrary(ItemViewModel item)
        {
            try
            {
                if (item.Download_url != null)
                {
                    var client = new HttpClient();
                    HttpRequestMessage request = new
                        HttpRequestMessage(HttpMethod.Get, item.Download_url);
                    var response = await client.
                        SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                    //var filename = item.Image_url.Substring(item.Image_url.LastIndexOf('/') + 1);
                    Guid photoID = System.Guid.NewGuid();
                    var filename = photoID.ToString() + ".mp3";
                    //var filename = Path.GetFileName(item.Image_url);
                    Task<StorageFile> task =
                        GetFileIfExistsAsync(KnownFolders.MusicLibrary, filename);
                    StorageFile file = await task;

                    if (file == null)
                    {
                        var imageFile = await KnownFolders.MusicLibrary.
                            CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                        var fs = await imageFile.OpenAsync(FileAccessMode.ReadWrite);
                        var writer = new DataWriter(fs.GetOutputStreamAt(0));

                        writer.WriteBytes(await response.Content.ReadAsByteArrayAsync());

                        await writer.StoreAsync();
                        writer.DetachStream();
                        await fs.FlushAsync();

                        var dialog = new MessageDialog("Music file is successfully saved in \"Music Library\".",
                            "Saving music");
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        var dialog = new MessageDialog("Music file is already saved in \"Music Library\".",
                            "Saving music");
                        await dialog.ShowAsync();
                    }
                }
                else 
                {
                    var dialog = new MessageDialog("Can't save file now.",
                            "Saving music");
                    await dialog.ShowAsync();
                };
            } catch {
                /*var dialog = new MessageDialog("Can't save file now.",
                            "Saving music");
                await dialog.ShowAsync();*/
            };
        }

        private void DataLoaded(object sender, EventArgs e)
        {
            try
            {
                this.Loading.IsIndeterminate = false;
                this.Loading.Visibility = Visibility.Collapsed;

                this.itemGridView.ItemsSource = App.ViewModel.Items;
                this.itemListView.ItemsSource = App.ViewModel.Items;
            }
            catch
            {
                this.Loading.IsIndeterminate = false;
                this.Loading.Visibility = Visibility.Collapsed;
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += Settings_CommandsRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested -= Settings_CommandsRequested;
        }

        void Settings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var viewAboutPage = new SettingsCommand("", "About", cmd =>
            {
                //(Window.Current.Content as Frame).Navigate(typeof(AboutPage));
                var settingsFlyout = new SettingsFlyout();
                settingsFlyout.Content = new About(); 
                settingsFlyout.HeaderText = "About";

                settingsFlyout.IsOpen = true;
            });
            args.Request.ApplicationCommands.Add(viewAboutPage);

            var viewAboutMalukahPage = new SettingsCommand("", "About Malukah", cmd =>
            {
                var settingsFlyout = new SettingsFlyout();
                settingsFlyout.Content = new AboutMalukah();
                settingsFlyout.HeaderText = "About Malukah";

                settingsFlyout.IsOpen = true;
            });
            args.Request.ApplicationCommands.Add(viewAboutMalukahPage);

            var PrivacyPage = new SettingsCommand("", "Privacy", cmd =>
            {
                var settingsFlyout = new SettingsFlyout();
                settingsFlyout.Content = new Privacy();
                settingsFlyout.HeaderText = "Privacy";

                settingsFlyout.IsOpen = true;
            });
            args.Request.ApplicationCommands.Add(PrivacyPage);
        }

        async private void OpenWebBrowser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri(((ItemViewModel)itemGridView.SelectedItem).Permalink_url));
            }
            catch { };
        }

        private void SaveMusicAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveAsMusicInMusicLibrary((ItemViewModel)itemGridView.SelectedItem);
            }
            catch { };
        }

        private void PlayAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Player.Stop();

                ItemViewModel item = (ItemViewModel)this.itemGridView.SelectedItem;
                Player.Source = new Uri(item.Download_url);
                Player.Play();
            }
            catch { };
        }

        private void PauseAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Player.Pause();
            }
            catch { };
        }


    }
}
