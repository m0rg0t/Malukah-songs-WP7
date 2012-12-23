// Copyright 2011 Google Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Google.AdMob.Ads.WindowsPhone7;
using Google.AdMob.Ads.WindowsPhone7.WPF;
using Microsoft.Phone.Controls;

namespace Google.AdMob.Ads.Sample.Intermediate
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void BuildNewAd(object sender, RoutedEventArgs e)
        {
            AdControl = new Google.AdMob.Ads.WindowsPhone7.WPF.BannerAd
            {
                // AdUnitID = "[Put your Ad Unit ID here and uncomment]",
                // If you want to use GPS, include the GeoProvider class in your project
                GpsLocation = UseGps.IsChecked.Value ? GeoProvider.DeviceLocation : null
            };

            // Can't forget to re-hook up all the events.
            AdControl.AdFailed += AdFailed;
            AdControl.AdLeavingApplication += AdLeavingApplication;
            AdControl.AdPresentingScreen += AdPresentingScreen;
            AdControl.AdReceived += AdReceived;

            // I didn't re-add the bindings, don't need the complexity in sample code

            // And add it to the display tree
            AdContainer.Children.Add(AdControl);
        }

        private void ClearAds(object sender, RoutedEventArgs e)
        {
            // We're just going to remove all of the banner ad objects
            foreach (BannerAd Ad in AdContainer.Children.Where(c => c is BannerAd).ToArray())
                AdContainer.Children.Remove(Ad);

            // ... and then create a new one so I don't have to do all sorts of null checks
            BuildNewAd(sender, e);
        }

        private void UseGps_Changed(object sender, RoutedEventArgs e)
        {
            // BeginUpdates() and EndUpdates() are optional, but they are generally a good idea
            // when updating multiple properties.  Updating a single property may trigger a new
            // ad, using Beg/EndUpdate() forces the ad control to wait until all properties have
            // been set.  
            AdControl.BeginUpdates();

            // Put your Ad Unit ID here and uncomment
            AdControl.AdUnitID = "";
            AdControl.GpsLocation =
                UseGps.IsChecked.Value ? GeoProvider.DeviceLocation : null;

            AdControl.EndUpdates();
        }

        private void AdFailed(object sender, AdException exception)
        {
            Notify("AdFailed - " + exception.Message);
            AdContainer.Children.Remove((UIElement)sender);
        }

        private void AdLeavingApplication(object sender, RoutedEventArgs e)
        {
            Notify("AdLeavingApplication");
        }

        private void AdPresentingScreen(object sender, RoutedEventArgs e)
        {
            Notify("AdPresentingScreen");
        }

        private void AdReceived(object sender, RoutedEventArgs e)
        {
            Notify("AdReceived");
        }

        private void Notify(string message)
        {
            LastEvent.Text = message;
            EventFader.Seek(TimeSpan.Zero);
            EventFader.Begin();
        }
    }
}