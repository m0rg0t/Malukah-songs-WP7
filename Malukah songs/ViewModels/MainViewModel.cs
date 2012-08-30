using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using System.Net;
using Newtonsoft.Json;
using System.IO.IsolatedStorage;


namespace Malukah_songs
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public IsolatedStorageSettings Storage = IsolatedStorageSettings.ApplicationSettings;
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();

            if (!this.Storage.Contains("favitems"))
            {
                this.Items = new ObservableCollection<ItemViewModel>();
            }
            else
            {
                Items = JsonConvert.DeserializeObject<ObservableCollection<ItemViewModel>>(this.Storage["favitems"].ToString());
            };
            //Items = JsonConvert.DeserializeObject<ObservableCollection<ItemViewModel>>(_test1);
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(messagesResponseCompleted);
            webClient.DownloadStringAsync(new Uri("http://api.soundcloud.com/users/malukah/tracks.json?client_id=c210a3efbb3d75200118f6bf24d71ee0"));

            
        }

        void messagesResponseCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //get downloaded string
            string json = e.Result.ToString();
            //string json = JsonConvert.SerializeObject(this.Items, Formatting.Indented);
            if (!this.Storage.Contains("favitems"))
            {
                this.Storage.Add("favitems", json);
            }
            else
            {
                this.Storage["favitems"] = json;
            }
            this.Storage.Save();

            Items = JsonConvert.DeserializeObject<ObservableCollection<ItemViewModel>>(json);
            this.IsDataLoaded = true;
            NotifyPropertyChanged("IsDataLoaded");
            NotifyPropertyChanged("Items");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}