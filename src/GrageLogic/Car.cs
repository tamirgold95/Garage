namespace Ex3.GarageLogic
{
    using System;

    public class Car : Vehicle
    {
        private eColor m_Color;
        private eNumberOfDoors m_NumberOfDoors;

        public Car(string i_ModelName, string i_LicenseNumber, string i_WheelsManufacturerName, Engine i_Engine)
            : base(i_ModelName, i_LicenseNumber, 4, i_WheelsManufacturerName, i_Engine) 
        {
            foreach (Wheel wheel in Wheels)
            {
                wheel.MaxAirPressure = 29;
            }

            if (i_Engine is FuelEngine)
            {
                (i_Engine as FuelEngine).m_FuelType = FuelEngine.eFuelType.Octane95;
                i_Engine.Max = 38;
            }
            else if (i_Engine is ElectricEngine)
            {
                i_Engine.Max = 3.3f;
            }
        }

        public eColor Color
        {
            get
            {
                return m_Color;
            }

            set
            {
                if (Enum.IsDefined(typeof(eColor), value))
                {
                    m_Color = value;
                }
                else
                {
                    throw new ArgumentException(string.Format("The color you entered is invalid."));
                }
            }
        }

        public eNumberOfDoors NumberOfDoors
        {
            get
            {
                return m_NumberOfDoors;
            }

            set
            {
                if (Enum.IsDefined(typeof(eNumberOfDoors), value))
                {
                    m_NumberOfDoors = value;
                }
                else
                {
                    throw new ArgumentException(string.Format("The number of doors you entered is invalid."));
                }
            }
        }

        public enum eColor
        {
            Red = 1,
            White,
            Green,
            Blue
        }

        public enum eNumberOfDoors
        {
            Two = 1,
            Three,
            Four,
            Five
        }

        public override string ToString()
        {
            string vehicleStringRep = string.Format(
                @"{0}
Car's color - {1}.
Car's number of doors- {2}.",
                base.ToString(),
                m_Color,
                m_NumberOfDoors);

            return vehicleStringRep;
        }
    }
}
