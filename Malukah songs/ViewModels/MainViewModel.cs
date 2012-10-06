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


        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get { return _items; } set { _items = value; } }
        private ObservableCollection<ItemViewModel> _items;

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
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(messagesResponseCompleted);
                webClient.DownloadStringAsync(new Uri("http://api.soundcloud.com/users/malukah/tracks.json?client_id=c210a3efbb3d75200118f6bf24d71ee0"));
            }
            catch { };
        }

        void messagesResponseCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var bw = new BackgroundWorker();            
            bw.DoWork += delegate
            {
                try
                {
                    //System.Threading.Thread.Sleep(900);
                    string json = e.Result.ToString();
                    if (!this.Storage.Contains("favitems"))
                    {
                        this.Storage.Add("favitems", json);
                    }
                    else
                    {
                        this.Storage["favitems"] = json;
                    }
                    this.Storage.Save();
                    this._items = JsonConvert.DeserializeObject<ObservableCollection<ItemViewModel>>(json);

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            this.IsDataLoaded = true;
                            NotifyPropertyChanged("IsDataLoaded");
                            NotifyPropertyChanged("Items");

                            App.ViewModel.OnDataLoad(EventArgs.Empty);
                        });
                }
                catch { };
            };
            bw.RunWorkerAsync();  
        }

        public delegate void DataLoadEventHandler(object sender, EventArgs e);
        public event DataLoadEventHandler DataLoad;
        // Invoke the Changed event; called whenever list changes
        protected virtual void OnDataLoad(EventArgs e)
        {

            if (DataLoad != null)
            {
                //this.NotifyPropertyChanged("Items");
                DataLoad(this, e);
            };
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