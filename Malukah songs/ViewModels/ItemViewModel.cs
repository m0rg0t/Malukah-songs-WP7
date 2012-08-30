using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Malukah_songs
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private string _Title;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
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