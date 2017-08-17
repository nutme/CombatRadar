namespace CombatRadar.Orienteering
{
    public class Coordinate
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public Coordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}