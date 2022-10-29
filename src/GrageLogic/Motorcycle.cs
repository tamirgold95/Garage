namespace Ex3.GarageLogic
{
    using System;

    public class Motorcycle : Vehicle
    {
        internal eLicenseType m_LicenseType;

        public int EngineVolume { get; set; }

        public Motorcycle(string i_ModelName, string i_LicenseNumber, string i_WheelsManufacturerName, Engine i_Engine)
            : base(i_ModelName, i_LicenseNumber, 2, i_WheelsManufacturerName, i_Engine)
        {
            foreach (Wheel wheel in Wheels)
            {
                wheel.MaxAirPressure = 31;
            }

            if (i_Engine is FuelEngine)
            {
                (i_Engine as FuelEngine).m_FuelType = FuelEngine.eFuelType.Octane98;
                i_Engine.Max = 6.2f;
            }
            else if (i_Engine is ElectricEngine)
            {
                i_Engine.Max = 2.5f;
            }
        }

        public eLicenseType LicenseType
        {
            get
            {
                return m_LicenseType;
            }

            set
            {
                if (Enum.IsDefined(typeof(eLicenseType), value))
                {
                    m_LicenseType = value;
                }
                else
                {
                    throw new ArgumentException(string.Format("The License Type you entered is invalid."));
                }
            }
        }

        public enum eLicenseType
        {
            A,
            A1,
            B1,
            BB
        }

        public override string ToString()
        {
            string vehicleStringRep = string.Format(
                @"{0}
Motorcycle's license type - {1}.
Motorcycle's engine volume- {2}.",
                base.ToString(),
                m_LicenseType,
                EngineVolume);

            return vehicleStringRep;
        }
    }
}
