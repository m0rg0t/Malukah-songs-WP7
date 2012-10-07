﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalukahSongs.DataModel
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private string _Title;

        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (value != _Title)
                {
                    _Title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        private string _Stream_url;

        public string Stream_url
        {
            get
            {
                return _Stream_url;
            }
            set
            {
                if (value != _Stream_url)
                {
                    _Stream_url = value + "?client_id=c210a3efbb3d75200118f6bf24d71ee0";
                    NotifyPropertyChanged("Stream_url");
                }
            }
        }

        private string _Download_url;

        public string Download_url
        {
            get
            {
                return _Download_url;
            }
            set
            {
                if (value != _Download_url)
                {
                    _Download_url = value.ToString() + "?client_id=c210a3efbb3d75200118f6bf24d71ee0";
                    NotifyPropertyChanged("Download_url");
                }
            }
        }

        private string _Permalink_url;

        public string Permalink_url
        {
            get
            {
                return _Permalink_url;
            }
            set
            {
                if (value != _Permalink_url)
                {
                    _Permalink_url = value;
                    NotifyPropertyChanged("Permalink_url");
                }
            }
        }


        private string _Waveform_url;

        public string Waveform_url
        {
            get
            {
                return _Waveform_url;
            }
            set
            {
                if (value != _Waveform_url)
                {
                    _Waveform_url = value;
                    NotifyPropertyChanged("Waveform_url");
                }
            }
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