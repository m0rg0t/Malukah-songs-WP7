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
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
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
            //MessageBox.Show(me.CurrentState.ToString());
            /*if (me.CurrentState.ToString() != "Playing")
            {
                me.Play();
                var bar = me.Tag as PerformanceProgressBar;
                bar.IsIndeterminate = true;
            }
            else
            {
                me.Stop();
                var bar = me.Tag as PerformanceProgressBar;
                bar.IsIndeterminate = false;
            };*/
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
            /*try
            {
                var me = ((FrameworkElement)sender).Tag as MediaElement;
                if (me.CurrentState.ToString() != "Playing")
                {
                    me.Play();
                    var bar = me.Tag as PerformanceProgressBar;
                    bar.IsIndeterminate = true;
                }
                else
                {
                    me.Stop();
                    var bar = me.Tag as PerformanceProgressBar;
                    bar.IsIndeterminate = false;
                };
            }
            catch { };*/
        }
    }
}