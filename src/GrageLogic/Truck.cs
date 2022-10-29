namespace Ex3.GarageLogic
{
    using System;

    public class Truck : Vehicle
    {
        public bool ContainsCooledCargo { get; set; }

        private float m_CargoTankVolume;

        public Truck(string i_ModelName, string i_LicenseNumber, string i_WheelsManufacturerName, Engine i_Engine)
            : base(i_ModelName, i_LicenseNumber, 16, i_WheelsManufacturerName, i_Engine)
        {
            foreach (Wheel wheel in Wheels)
            {
                wheel.MaxAirPressure = 24;
            }

            if (i_Engine is FuelEngine)
            {
                (i_Engine as FuelEngine).m_FuelType = FuelEngine.eFuelType.Soler;
                i_Engine.Max = 120;
            }
        }

        public float CargoTankVolume
        {
            get
            {
                return m_CargoTankVolume;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException(string.Format("The cargo tank volume should be above 0."));
                }

                m_CargoTankVolume = value;
            }
        }

        public override string ToString()
        {
            string DoesOrDoesntContainCooledCargo = string.Empty;
            if (ContainsCooledCargo)
            {
                DoesOrDoesntContainCooledCargo = "Does";
            }
            else
            {
                DoesOrDoesntContainCooledCargo = "Doesn't";
            }

            string vehicleStringRep = string.Format(
                @"{0}
Truck's {1} contain cooled cargo.
Truck's cargo tank volume - {2}.",
                base.ToString(),
                DoesOrDoesntContainCooledCargo,
                m_CargoTankVolume);

            return vehicleStringRep;
        }
    }
}