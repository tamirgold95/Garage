namespace Ex3.GarageLogic
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Garage
    {
        public Dictionary<string, Vehicle> VehiclesInGarage { get; set; }

        public Garage()
        {
            VehiclesInGarage = new Dictionary<string, Vehicle>();
        }

        public StringBuilder GetLicensesInGarage(int i_FilterByStatus)
        {
            StringBuilder licensesInGarage = new StringBuilder();
            if (i_FilterByStatus == 4)
            {
                foreach (string licenseNumber in VehiclesInGarage.Keys)
                {
                    licensesInGarage.Append(licenseNumber + "\n");
                }
            }
            else if (Enum.IsDefined(typeof(Vehicle.eStatus), i_FilterByStatus))
            {
                foreach (Vehicle vehicle in VehiclesInGarage.Values)
                {
                    if ((int)vehicle.Status == i_FilterByStatus)
                    {
                        licensesInGarage.Append(vehicle.LicenseNumber + "\n");
                    }
                }
            }
            else
            {
                throw new ArgumentException(string.Format("Invalid status."));
            }

            return licensesInGarage;
        }

        public void UpdateStatus(string i_LicenseNumber, Vehicle.eStatus i_Status)
        {
            VehiclesInGarage[i_LicenseNumber].Status = i_Status;
        }

        public void InflateTiresToMax(string i_LicenseNumber)
        {
            if (VehiclesInGarage.ContainsKey(i_LicenseNumber))
            {
                foreach (Wheel wheel in VehiclesInGarage[i_LicenseNumber].Wheels)
                {
                    wheel.CurrentAirPressure = wheel.MaxAirPressure;
                }
            }
            else
            {
                throw new ArgumentException(string.Format("License number not found."));
            }
        }

        public void Refuel(string i_LicenseNumber, FuelEngine.eFuelType i_FuelType, float i_FuelToAdd)
        {
            (VehiclesInGarage[i_LicenseNumber].Engine as FuelEngine).Refuel(i_FuelToAdd, i_FuelType.ToString());
        }

        public void Charge(string i_LicenseNumber, int i_MinutesToCharge)
        {
            float hoursToCharge = i_MinutesToCharge / 60f;

            (VehiclesInGarage[i_LicenseNumber].Engine as ElectricEngine).Recharge(hoursToCharge);
        }
    }
}
