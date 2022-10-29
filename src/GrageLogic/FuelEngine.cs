namespace Ex3.GarageLogic
{
    using System;

    public class FuelEngine : Engine
    {
        public eFuelType m_FuelType { get; set; }

        public FuelEngine(float i_Remaining = 0)
            : base(i_Remaining)
        {
        }

        public void Refuel(float i_FuelToAdd, string i_ProvidedFuelType)
        {
            if (m_FuelType.ToString() != i_ProvidedFuelType)
            {
                throw new ArgumentException(string.Format("Provided fuel doesn't match."));
            }

            if (Remaining + i_FuelToAdd <= Max)
            {
                Remaining += i_FuelToAdd;
            }
            else
            {
                float amountBeforeRefuel = Remaining;

                Remaining = Max;
                throw new ValueOutOfRangeException(i_FuelToAdd, amountBeforeRefuel, Max);
            }
        }

        public enum eFuelType
        {
            Soler = 1,
            Octane95,
            Octane96,
            Octane98
        }
    }
}