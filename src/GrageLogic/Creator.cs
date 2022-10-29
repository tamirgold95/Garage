namespace Ex3.GarageLogic
{
    using System;

    public class Creator
    {
        public static Vehicle CreateVehicle(eVehicle i_VehicleType, string i_ModelName, string i_LicenseNumber, string i_WheelsManufacturerName, Engine i_Engine)
        {
            Vehicle newVehicle = null;

            switch (i_VehicleType)
            {
                case eVehicle.FuelCar:
                    newVehicle = new Car(i_ModelName, i_LicenseNumber, i_WheelsManufacturerName, i_Engine);
                    break;
                case eVehicle.ElectricCar:
                    newVehicle = new Car(i_ModelName, i_LicenseNumber, i_WheelsManufacturerName, i_Engine);
                    break;
                case eVehicle.FuelMotorcycle:
                    newVehicle = new Motorcycle(i_ModelName, i_LicenseNumber, i_WheelsManufacturerName, i_Engine);
                    break;
                case eVehicle.ElectricMotorcycle:
                    newVehicle = new Motorcycle(i_ModelName, i_LicenseNumber, i_WheelsManufacturerName, i_Engine);
                    break;
                case eVehicle.FuelTruck:
                    newVehicle = new Truck(i_ModelName, i_LicenseNumber, i_WheelsManufacturerName, i_Engine);
                    break;
                default:
                    throw new Exception("Invalid vehicle.");
            }

            return newVehicle;
        }

        public enum eVehicle
        {
            FuelCar = 1,
            ElectricCar,
            FuelMotorcycle,
            ElectricMotorcycle,
            FuelTruck,
        }
    }
}
