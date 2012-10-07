using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MalukahSongs.DataModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings; 

        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();

            try
            {
                string json = (string)roamingSettings.Values["tracks"];
                Items = JsonConvert.DeserializeObject<ObservableCollection<ItemViewModel>>(json);
            }
            catch { };
        }

        public ObservableCollection<ItemViewModel> Items { get; private set; }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async void LoadData()
        {
            string json = "";

            json = await MakeWebRequestForSoundcloud();

            roamingSettings.Values["tracks"] = json; 

            Items = JsonConvert.DeserializeObject<ObservableCollection<ItemViewModel>>(json);
            this.IsDataLoaded = true;
            NotifyPropertyChanged("IsDataLoaded");
            NotifyPropertyChanged("Items");
        }

        public async Task<string> MakeWebRequestForSoundcloud()
        {
            HttpClient http = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await http.GetAsync("http://api.soundcloud.com/users/malukah/tracks.json?client_id=c210a3efbb3d75200118f6bf24d71ee0");
            return await response.Content.ReadAsStringAsync();
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
