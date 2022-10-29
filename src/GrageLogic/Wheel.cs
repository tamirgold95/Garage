namespace Ex3.GarageLogic
{
    public class Wheel
    {
        private readonly string r_ManufacturerName;

        public float MaxAirPressure { get; set; }

        public float CurrentAirPressure { get; set; }

        internal Wheel(string i_ManufacturerName)
        {
            r_ManufacturerName = i_ManufacturerName;
        }

        internal string ManufacturerName
        {
            get { return r_ManufacturerName; }
        }

        public void Inflate(float i_AirToAdd)
        {
            if (CurrentAirPressure + i_AirToAdd <= MaxAirPressure)
            {
                CurrentAirPressure += i_AirToAdd;
            }
            else
            {
                float amountBeforeInflating = CurrentAirPressure;
                CurrentAirPressure = MaxAirPressure;
                throw new ValueOutOfRangeException(i_AirToAdd, amountBeforeInflating, MaxAirPressure);
            }
        }
    }
}
