namespace CombatRadar
{
    using System.Linq;
    using Android.Locations;
    using Android.App;
    using Android.Widget;
    using Android.OS;

    public class DistanceControl
    {
        private readonly ProgressBar _targetDistanceProgressControl;

        public DistanceControl(ProgressBar targetDistanceProgressControl)
        {
            _targetDistanceProgressControl = targetDistanceProgressControl;
            _targetDistanceProgressControl.Max = 100;
        }

        public void UpdateDistance(Location from, Location to)
        {
            _targetDistanceProgressControl.Progress = 50;
        }
    }

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

            _distanceControl = new DistanceControl(targetDistanceProgressControl);
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
                _status.SetStatus("GPS enabled");
                _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
            } else {
                _gpsEnabled = false;
                _status.SetStatus("GPS disabled");
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
            _status.SetStatus("Location provider disabled");
        }

        public void OnProviderEnabled(string provider)
        {
            _status.SetStatus("Location provider enabled");
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            _status.SetStatus($"Location status cjanhed: {status}");
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
                _distanceControl.UpdateDistance(location, location);
                var coordinatesAsString = $"lat: {_currentLocation.Latitude:f6}, long: {_currentLocation.Longitude:f6}";
                _status.SetStatus(coordinatesAsString);
            }
        }
    }
}

