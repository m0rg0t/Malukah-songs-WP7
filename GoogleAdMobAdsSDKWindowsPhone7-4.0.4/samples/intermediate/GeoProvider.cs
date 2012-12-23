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
using System.Device.Location;
using Google.AdMob.Ads.WindowsPhone7;

namespace Google.AdMob.Ads.Sample.Intermediate
{
    /// <summary>
    /// Provides a simple mechanism for getting the current GPS coordinates
    /// </summary>
    public class GeoProvider
    {
        static GeoProvider()
        {
            try
            {
                DeviceLocation = null;

                GeoCoordinateWatcher Provider = new GeoCoordinateWatcher();
                Provider.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(GeoProvider_PositionChanged);
                Provider.Start(true);

                System.Diagnostics.Debug.WriteLine("Geo:   Location Available, Current Position: {0}", Provider.Position.Location);
            }
            catch (Exception exx)
            {
                System.Diagnostics.Debug.WriteLine("Geo:   Location Not Available");
                System.Diagnostics.Debug.WriteLine(exx);
            }
        }

        private static void GeoProvider_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (e.Position.Location.IsUnknown)
                DeviceLocation = null;
            else
                DeviceLocation = new GpsLocation
                {
                    Latitude = e.Position.Location.Longitude,
                    Longitude = e.Position.Location.Longitude,
                    Accuracy = e.Position.Location.HorizontalAccuracy
                };
        }

        /// <summary>
        /// Provides the current GPS coordinates, if available
        /// </summary>
        public static GpsLocation? DeviceLocation { get; private set; }
    }
}