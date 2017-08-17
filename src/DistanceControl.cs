namespace CombatRadar
{
    using Android.Locations;
    using Android.Widget;
    using Orienteering;

    public class DistanceControl
    {
        private readonly ProgressBar _targetDistanceProgressControl;
        private readonly TextView _debugDistanceControl;

        private Coordinate targetCoordinate;

        public DistanceControl(ProgressBar targetDistanceProgressControl, TextView debugDistanceControl)
        {
            _targetDistanceProgressControl = targetDistanceProgressControl;
            _debugDistanceControl = debugDistanceControl;

            targetCoordinate = new Coordinate(51.5205666, -0.0952953);
        }

        public void UpdateDistance(Location from)
        {
            var fromCoordinate = new Coordinate(from.Latitude, from.Longitude);

            var distanceInMeters = DistanceCalculator.DistanceTo(fromCoordinate, targetCoordinate, UnitOfLength.Meters);
            _debugDistanceControl.Text = $"{distanceInMeters}m";

            // progress bar is inactive for target beyong 500m
            // between 500 meters and 50 meters it moves slowly
            // between 50 meters and 0 meters it moves faster

            var progress = 0;

            if (distanceInMeters <= 500 && distanceInMeters > 50)
            {
                progress = (int) (50 - distanceInMeters/10);
            }
            else if (distanceInMeters < 50)
            {
                progress = (int) (100 - distanceInMeters);
            }

            _targetDistanceProgressControl.Progress = progress;
            _debugDistanceControl.Text = $"Distance: {distanceInMeters}m progress: {progress}";
        }
    }
}