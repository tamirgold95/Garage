namespace Ex3.GarageLogic
{
    using System.Collections.Generic;
    using System.Text;

    public abstract class Vehicle
    {
        private readonly string r_ModelName;
        private readonly string r_LicenseNumber;

        internal float RemainingEnergyPercentage { get; set; }

        public List<Wheel> Wheels { get; set; } = new List<Wheel>();

        public string OwnerName { get; set; }

        public string OwnerPhoneNumber { get; set; }

        public eStatus Status { get; set; } = eStatus.InRepair;

        public Engine Engine { get; set; }

        public Vehicle(string i_ModelName, string i_LicenseNumber, int i_NumOfWheels, string i_WheelsManufacturerName, Engine i_Engine)
        {
            r_ModelName = i_ModelName;
            r_LicenseNumber = i_LicenseNumber;
            RemainingEnergyPercentage = i_Engine.GetEnergyPercentage();
            Engine = i_Engine;
            for (int i = 0; i < i_NumOfWheels; i++)
            {
                Wheels.Add(new Wheel(i_WheelsManufacturerName));
            }
        }

        public string ModelName
        {
            get { return r_ModelName; }
        }

        public string LicenseNumber
        {
            get { return r_LicenseNumber; }
        }

        public enum eStatus
        {
            InRepair = 1,
            Repaired,
            PaidFor
        }

        public override string ToString()
        {
            int i = 0;
            string currentWheelAirPressure = string.Empty;
            StringBuilder wheelsAirPressure = new StringBuilder();
            string engineStringRep = string.Empty;

            foreach (Wheel wheel in Wheels)
            {
                currentWheelAirPressure = string.Format("Tire {0}'s air pressure is {1} psi.\n", i++, wheel.CurrentAirPressure);
                wheelsAirPressure.Append(currentWheelAirPressure);
            }

            if (Engine is ElectricEngine)
            {
                engineStringRep = string.Format(@"Battery status - {0}.", Engine.Remaining);
            }
            else
            {
                engineStringRep = string.Format(
                    @"Fuel status - {0}.
Fuel type - {1}.",
                    Engine.Remaining,
                    (Engine as FuelEngine).m_FuelType);
            }

            string vehicleStringRep = string.Format(
                @"Vehicle information:
License number - {0}.
Model name - {1}.
Owner name - {2}.
Status in garage - {3}.
Tires manufacturer - {4}.
{5}
{6}",
                LicenseNumber,
                ModelName,
                OwnerName,
                Status,
                Wheels[0].ManufacturerName,
                wheelsAirPressure,
                engineStringRep);

            return vehicleStringRep;
        }
    }
}
