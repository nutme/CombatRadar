namespace CombatRadar
{
    using System.Linq;
    using Android.Locations;
    using Android.App;
    using Android.Widget;
    using Android.OS;
    using Android.Views;

    [Activity(Label = "Combat Radar", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ILocationListener
    {
        private Status _status;
        private DistanceControl _distanceControl;
        Location _currentLocation;
        LocationManager _locationManager;
        private string _locationProvider;
        private bool _gpsEnabled;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);
            InitializeLocationManager();

            // Get the UI controls from the loaded layout
            var statusTextControl = FindViewById<TextView>(Resource.Id.statusTextView);
            var gpsSwitchControl = FindViewById<Switch>(Resource.Id.gpsSwitch);
            var targetDistanceProgressControl = FindViewById<ProgressBar>(Resource.Id.targetDistanceProgressBar);
            var debugDistanceControl = FindViewById<TextView>(Resource.Id.debugDistanceTextView);
            var targetDistanceHintControl = FindViewById<TextView>(Resource.Id.targetDistanceHintText);

            // Only for debugging porpuses
            //debugDistanceControl.Visibility = ViewStates.Visible;

            _distanceControl = new DistanceControl(targetDistanceProgressControl, targetDistanceHintControl, debugDistanceControl);
            _status = new Status(statusTextControl);
            _status.SetStatus("Ready!");

            gpsSwitchControl.CheckedChange += ToggleGps;
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (_gpsEnabled)
            {
                _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            if (_gpsEnabled)
            {
                _locationManager.RemoveUpdates(this);
            }
        }

        private void ToggleGps(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {
                _gpsEnabled = true;
                _status.SetStatus("Locating position");
                _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
            } else {
                _gpsEnabled = false;
                _status.SetStatus("Ready!");
                _locationManager.RemoveUpdates(this);
            }
        }

        private void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            var criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };

            var acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);
            _locationProvider = acceptableLocationProviders.Any() ? acceptableLocationProviders.First() : string.Empty;
        }

        public void OnLocationChanged(Location location)
        {
            UpdateCurrentLocation(location);
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            _status.SetStatus("Active");
        }

        private void UpdateCurrentLocation(Location location)
        {
            _currentLocation = location;
            if (_currentLocation == null)
            {
                _status.SetStatus("Unable to update your location");
            }
            else
            {
                _distanceControl.UpdateDistance(location);
                _status.SetStatus("Active");
            }
        }
    }
}

