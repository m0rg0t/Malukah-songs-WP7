using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Net.NetworkInformation;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Malukah_songs
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                bool hasNetworkConnection = NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None;
                if (hasNetworkConnection)
                {
                    this.progressOverlay.Visibility = Visibility.Visible;
                    this.progressOverlay.IsEnabled = true;
                }
                else
                {
                    /// DWP-95
                    MessageBox.Show("No internet connection, can't load fresh songs list.");

                    if (!App.ViewModel.Storage.Contains("favitems"))
                    {
                        App.ViewModel.Items = new ObservableCollection<ItemViewModel>();
                    }
                    else
                    {
                        App.ViewModel.Items = JsonConvert.DeserializeObject<ObservableCollection<ItemViewModel>>(App.ViewModel.Storage["favitems"].ToString());
                    };
                    this.SongList.ItemsSource = App.ViewModel.Items;

                    this.progressOverlay.Visibility = Visibility.Collapsed;
                    this.progressOverlay.IsEnabled = false;
                };
                if (!App.ViewModel.IsDataLoaded)
                {
                    try
                    {
                        App.ViewModel.LoadData();
                        App.ViewModel.DataLoad += new MainViewModel.DataLoadEventHandler(this.DataLoaded);
                    }
                    catch
                    {
                        this.progressOverlay.Visibility = Visibility.Collapsed;
                        this.progressOverlay.IsEnabled = false;
                    };
                }
            }
            catch { };
        }

        private void DataLoaded(object sender, EventArgs e)
        {
            try
            {
                this.SongList.ItemsSource = App.ViewModel.Items;

                this.progressOverlay.Visibility = Visibility.Collapsed;
                this.progressOverlay.IsEnabled = false;
            }
            catch
            {
                this.progressOverlay.Visibility = Visibility.Collapsed;
                this.progressOverlay.IsEnabled = false;
            };
        }

        private void YoutubeImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                WebBrowserTask webbrowser = new WebBrowserTask();
                webbrowser.Uri = new Uri("http://www.youtube.com/watch?v=4z9TdDCWN7g");
                webbrowser.Show();
            }
            catch { };
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var me = ((FrameworkElement)sender).Tag as MediaElement;
            try
            {
                try
                {
                    WebBrowserTask webbrowser = new WebBrowserTask();
                    webbrowser.Uri = new Uri((this.SongList.SelectedItem as ItemViewModel).Download_url.ToString());
                    webbrowser.Show();
                }
                catch {
                    WebBrowserTask webbrowser = new WebBrowserTask();
                    webbrowser.Uri = new Uri((this.SongList.SelectedItem as ItemViewModel).Stream_url.ToString());
                    webbrowser.Show();
                };
            }
            catch { };
        }

        private void Image_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                try
                {
                    WebBrowserTask webbrowser = new WebBrowserTask();
                    webbrowser.Uri = new Uri((this.SongList.SelectedItem as ItemViewModel).Download_url.ToString());
                    webbrowser.Show();
                }
                catch
                {
                    WebBrowserTask webbrowser = new WebBrowserTask();
                    webbrowser.Uri = new Uri((this.SongList.SelectedItem as ItemViewModel).Stream_url.ToString());
                    webbrowser.Show();
                };
            }
            catch { };
        }
    }
}