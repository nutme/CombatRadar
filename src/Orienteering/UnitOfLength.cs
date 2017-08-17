namespace CombatRadar.Orienteering
{
    public class UnitOfLength
    {
        public static UnitOfLength Meters = new UnitOfLength(1609.344);
        public static UnitOfLength Kilometers = new UnitOfLength(1.609344);
        public static UnitOfLength NauticalMiles = new UnitOfLength(0.8684);
        public static UnitOfLength Miles = new UnitOfLength(1);

        private readonly double _factor;

        private UnitOfLength(double factor)
        {
            _factor = factor;
        }

        public double ConvertFromMiles(double input)
        {
            return input*_factor;
        }
    }
}
