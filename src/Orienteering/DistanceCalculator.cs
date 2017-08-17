namespace CombatRadar.Orienteering
{
    using Java.Lang;

    public static class DistanceCalculator
    {
        /// <summary>
        /// Calculates distance between two coordinates.
        /// Returns distance in meters
        /// </summary>
        public static double DistanceTo(Coordinate fromCoordinates, Coordinate toCoordinates)
        {
            return DistanceTo(fromCoordinates, toCoordinates, UnitOfLength.Meters);
        }

        /// <summary>
        /// Calculates distance between two coordinates.
        /// </summary>
        public static double DistanceTo(Coordinate fromCoordinates, Coordinate toCoordinates, UnitOfLength unitOfLength)
        {
            var baseRad = Math.Pi * fromCoordinates.Latitude / 180;
            var targetRad = Math.Pi * toCoordinates.Latitude / 180;
            var theta = fromCoordinates.Longitude - toCoordinates.Longitude;
            var thetaRad = Math.Pi * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.Pi;
            dist = dist * 60 * 1.1515;

            return unitOfLength.ConvertFromMiles(dist);
        }
    }
}