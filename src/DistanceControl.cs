namespace CombatRadar
{
    using Android.Locations;
    using Android.Widget;
    using Orienteering;

    public class DistanceControl
    {
        private readonly ProgressBar _targetDistanceProgressControl;
        private readonly TextView _targetDistanceHintControl;
        private readonly TextView _debugDistanceControl;

        private Coordinate targetCoordinate;

        public DistanceControl(ProgressBar targetDistanceProgressControl, TextView targetDistanceHintControl, TextView debugDistanceControl)
        {
            _targetDistanceProgressControl = targetDistanceProgressControl;
            _targetDistanceHintControl = targetDistanceHintControl;
            _debugDistanceControl = debugDistanceControl;

            // barbican office
            //targetCoordinate = new Coordinate(51.5205666, -0.0952953);

            // gravesend train station
            targetCoordinate = new Coordinate(51.4418, 0.36586);
        }

        public void UpdateDistance(Location from)
        {
            var fromCoordinate = new Coordinate(from.Latitude, from.Longitude);

            var distanceInMeters = DistanceCalculator.DistanceTo(fromCoordinate, targetCoordinate, UnitOfLength.Meters);
            _debugDistanceControl.Text = $"{distanceInMeters}m";

            // progress bar is inactive for target beyong 500m
            // between 500 meters and 50 meters it moves slowly
            // between 50 meters and 0 meters it moves faster

            int progress;

            if (distanceInMeters <= 500 && distanceInMeters > 50)
            {
                progress = (int) (50 - distanceInMeters/10);
                _targetDistanceHintControl.Text = "Target is near by..";
            }
            else if (distanceInMeters < 50)
            {
                progress = (int) (100 - distanceInMeters);
                _targetDistanceHintControl.Text = "Target is very close!";
            }
            else
            {
                progress = 0;
                _targetDistanceHintControl.Text = "Target is to far. Need to get closer.";
            }

            _targetDistanceProgressControl.Progress = progress;
            _debugDistanceControl.Text = $"Distance: {distanceInMeters}m progress: {progress}";
        }
    }
}