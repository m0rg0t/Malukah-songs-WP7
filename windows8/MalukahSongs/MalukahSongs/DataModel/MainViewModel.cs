using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MalukahSongs.DataModel

{
    [Windows.Foundation.Metadata.WebHostHidden]
    public class MainViewModel : MalukahSongs.Common.BindableBase
    {
        Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings; 

        public MainViewModel()
        {
            Items.CollectionChanged += ItemsCollectionChanged;

            this._items = new ObservableCollection<ItemViewModel>();

            try
            {
                string json = (string)roamingSettings.Values["tracks"];
                _items = JsonConvert.DeserializeObject<ObservableCollection<ItemViewModel>>(json);

            }
            catch { };
        }

        public delegate void DataLoadEventHandler(object sender, EventArgs e);
        public event DataLoadEventHandler DataLoad;
        protected virtual void OnDataLoad(EventArgs e)
        {

            if (DataLoad != null)
            {
                //this.NotifyPropertyChanged("Items");
                DataLoad(this, e);
            };
        }

        private ObservableCollection<ItemViewModel> _items = new ObservableCollection<ItemViewModel>();
        public ObservableCollection<ItemViewModel> Items
        {
            get { return this._items; }
            
        }

        private ObservableCollection<ItemViewModel> _topItem = new ObservableCollection<ItemViewModel>();
        public ObservableCollection<ItemViewModel> TopItems
        {
            get { return this._topItem; }
        }

        

        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Предоставляет подмножество полной коллекции элементов, привязываемой из объекта GroupedItemsPage
            // по двум причинам: GridView не виртуализирует большие коллекции элементов и оно
            // улучшает работу пользователей при просмотре групп с большим количеством
            // элементов.
            //
            // Отображается максимальное число столбцов (12), поскольку это приводит к заполнению столбцов сетки
            // сколько строк отображается: 1, 2, 3, 4 или 6

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        if (TopItems.Count > 12)
                        {
                            TopItems.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 12)
                        {
                            TopItems.Add(Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 12)
                    {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
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
        public async void LoadData()
        {
            string json = "";

            json = await MakeWebRequestForSoundcloud();

            try
            {
                roamingSettings.Values["tracks"] = json;
            }
            catch { };

            _items = JsonConvert.DeserializeObject<ObservableCollection<ItemViewModel>>(json);

            /*NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, _items);
            this.ItemsCollectionChanged(this, e);*/

            this.IsDataLoaded = true;
            NotifyPropertyChanged("IsDataLoaded");
            NotifyPropertyChanged("Items");

            App.ViewModel.OnDataLoad(EventArgs.Empty);
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
