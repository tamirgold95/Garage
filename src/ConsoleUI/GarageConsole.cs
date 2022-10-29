namespace Ex3.ConsoleUI
{
    using System;
    using System.Text;
    using System.Reflection;
    using Ex3.GarageLogic;

    public class GarageConsole
    {
        public static void GarageUserInterface()
        {
            int userSelection;
            bool wantToStay = true;
            Garage currentGarage = new Garage();

            while (wantToStay)
            {
                MainScreen();
                try
                {
                    bool isNumeric = int.TryParse(Console.ReadLine(), out userSelection);
                    if (!isNumeric || !Enum.IsDefined(typeof(eMenuOptions), userSelection))
                    {
                        throw new FormatException();
                    }

                    switch (userSelection)
                    {
                        case 1:
                            InsertVehicle(currentGarage);
                            break;
                        case 2:
                            DisplayLicensesInGarage(currentGarage);
                            break;
                        case 3:
                            ChangeVehicleStatus(currentGarage);
                            break;
                        case 4:
                            InflateTiresToMaximum(currentGarage);
                            break;
                        case 5:
                            RefuelVehicle(currentGarage);
                            break;
                        case 6:
                            RechargeVehicle(currentGarage);
                            break;
                        case 7:
                            DisplayVehicleInfo(currentGarage);
                            break;
                        case 8:
                            wantToStay = false;
                            break;
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(string.Format(
                        @"{0} 
please try again", 
                        ex.Message));
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(string.Format(
                        @"{0} 
please try again",
                        ex.Message));
                }
                catch (ValueOutOfRangeException ex)
                {
                    Console.WriteLine(string.Format(@"{0}", ex.Message));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format(
                        @"{0} 
please try again", 
                        ex.Message));
                }
            }
        }

        public static void MainScreen()
        {
            Console.WriteLine(string.Format(@"
Welcome to our garage.
Please enter a number for each of the following actions:
1 - Insert a vehicle to garage.
2 - Display a list of license numbers currently in the garage.
3 - Change vehicle’s status.
4 - Inflate tires to maximum.
5 - Refuel a fuel-based vehicle.
6 - Charge an electric-based vehicle.
7 - Display vehicle information.
8 - Exit."));
        }

        public static void InsertVehicle(Garage io_CurrentGarage)
        {
            Console.WriteLine("Please enter license number: ");
            string licenseNumber = Console.ReadLine();

            Console.Clear();
            if (io_CurrentGarage.VehiclesInGarage.ContainsKey(licenseNumber))
            {
                io_CurrentGarage.VehiclesInGarage[licenseNumber].Status = Vehicle.eStatus.InRepair;
            }
            else
            {
                Console.WriteLine("Please enter the vehicle's owner name.");
                string ownerName = Console.ReadLine();

                Console.Clear();
                Console.WriteLine("Please enter the vehicle's owner phone number.");
                string ownerPhoneNumber = Console.ReadLine();

                while (!int.TryParse(ownerPhoneNumber, out _)) 
                {
                    Console.WriteLine("Please make sure to enter numbers only.");
                    ownerPhoneNumber = Console.ReadLine();
                }

                Console.Clear();
                Vehicle currentVehicle = null;
                Creator.eVehicle vehicleType = ReceiveVehcileType();

                Console.WriteLine("Please enter vehicle model name.");
                string modelName = Console.ReadLine();

                Console.Clear();
                Console.WriteLine("Please enter vehicle's wheels manufacturer name.");
                string wheelsManufacturerName = Console.ReadLine();

                Console.Clear();
                string engineType = GetVehicleTypeFirstWord(vehicleType);
                Engine currentEngine = null;

                if (engineType == "Fuel")
                {
                    currentEngine = new FuelEngine();
                }
                else if (engineType == "Electric")
                {
                    currentEngine = new ElectricEngine();
                }

                currentVehicle = Creator.CreateVehicle(vehicleType, modelName, licenseNumber, wheelsManufacturerName, currentEngine);
                currentVehicle.OwnerName = ownerName;
                currentVehicle.OwnerPhoneNumber = ownerPhoneNumber;
                currentVehicle.Engine.Remaining = ReceiveRemainingEnergy(currentVehicle);
                Console.Clear();
                ReceiveTiresAirPressure(currentVehicle);
                Console.Clear();
                UpdateUniqeProperties(currentVehicle);
                io_CurrentGarage.VehiclesInGarage.Add(licenseNumber, currentVehicle);
            }
        }

        public static void UpdateUniqeProperties(Vehicle io_CurrentVehicle)
        {
            Type typeOfVehicle = io_CurrentVehicle.GetType();
            PropertyInfo[] allProperties = typeOfVehicle.GetProperties(BindingFlags.Public
| BindingFlags.NonPublic
| BindingFlags.Instance
| BindingFlags.DeclaredOnly);
            foreach (PropertyInfo property in allProperties)
            {
                try
                {
                    if (property.PropertyType == typeof(bool))
                    {
                        Console.WriteLine("Enter 'true' if the vehicle{0}. Enter 'false' otherwise.", SplitByCapitalLetters(property.Name).ToLower());
                    }
                    else
                    {
                        Console.WriteLine("Please enter vehicle's{0}.", SplitByCapitalLetters(property.Name).ToLower());
                    }

                    if (property.PropertyType.IsEnum)
                    {
                        Console.WriteLine("Choose a number for each of the following options: ");
                        PrintEnum(property.PropertyType);
                        string userEnumInput = Console.ReadLine();
                        int userEnumNumber;
                        bool isNumericInput = int.TryParse(userEnumInput, out userEnumNumber);

                        while (!isNumericInput || !Enum.IsDefined(property.PropertyType, userEnumNumber))
                        {
                            Console.WriteLine("Invalid input, please try again.");
                            userEnumInput = Console.ReadLine();
                            isNumericInput = int.TryParse(userEnumInput, out userEnumNumber);
                        }

                        property.SetValue(io_CurrentVehicle, userEnumNumber);
                    }
                    else
                    {
                        string userInput = Console.ReadLine();
                        object underlyingValue = Convert.ChangeType(userInput, property.PropertyType);

                        property.SetValue(io_CurrentVehicle, underlyingValue);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            Console.Clear();
        }

        public static void PrintEnum(Type i_Enum)
        {
            string[] enumNames = Enum.GetNames(i_Enum);
            int i = 1;

            foreach (string enumValue in enumNames)
            {
                Console.WriteLine("{0} -{1}", i++, SplitByCapitalLetters(enumValue));
            }
        }

        public static void DisplayLicensesInGarage(Garage i_CurrentGarage)
        {
            Console.WriteLine(string.Format(@"Please enter a number to display only license numbers of vehicles that are:
1 - In Repair.
2 - Repaired.
3 - Paid For.
If you would like to see all license numbers enter 4."));
            string userInput = Console.ReadLine();
            int userSelection;
            bool isNumeric = int.TryParse(userInput, out userSelection);

            while (!isNumeric || !Enum.IsDefined(typeof(Vehicle.eStatus), userSelection))
            {
                if (userSelection == 4)
                {
                    break;
                }

                Console.WriteLine("Invalid input, please try again.");
                userInput = Console.ReadLine();
                isNumeric = int.TryParse(userInput, out userSelection);
            }

            Console.WriteLine(i_CurrentGarage.GetLicensesInGarage(userSelection));
        }

        public static void ChangeVehicleStatus(Garage i_CurrentGarage)
        {
            Console.WriteLine("Please enter license number: ");
            string licenseNumber = Console.ReadLine();

            while (!i_CurrentGarage.VehiclesInGarage.ContainsKey(licenseNumber))
            {
                Console.WriteLine("License number not found, please try again.");
                licenseNumber = Console.ReadLine();
            }

            Console.WriteLine(string.Format(@"Please enter one of the following numbers for desired status:
1 - In Repair.
2 - Repaired.
3 - Paid For."));
            string userInput = Console.ReadLine();
            int userSelection;
            bool isNumeric = int.TryParse(userInput, out userSelection);

            while (!isNumeric || !Enum.IsDefined(typeof(Vehicle.eStatus), userSelection))
            {
                Console.WriteLine("Invalid input, please try again.");
                userInput = Console.ReadLine();
                isNumeric = int.TryParse(userInput, out userSelection);
            }

            Vehicle.eStatus userStatus = ConvertToEnum<Vehicle.eStatus>(userSelection);
            i_CurrentGarage.UpdateStatus(licenseNumber, userStatus);
            Console.WriteLine("Status successfully changed to {0}.", userStatus);
        }

        public static void InflateTiresToMaximum(Garage i_CurrentGarage)
        {
            Console.WriteLine("Please enter license number: ");
            string licenseNumber = Console.ReadLine();

            i_CurrentGarage.InflateTiresToMax(licenseNumber);
            Console.WriteLine("All tires were inflated.");
        }

        public static void RefuelVehicle(Garage i_CurrentGarage)
        {
            Console.WriteLine("Please enter license number: ");
            string licenseNumber = Console.ReadLine();

            while (!i_CurrentGarage.VehiclesInGarage.ContainsKey(licenseNumber))
            {
                Console.WriteLine("License number not found, please try again.");
                licenseNumber = Console.ReadLine();
            }

            Vehicle currentVehicle = i_CurrentGarage.VehiclesInGarage[licenseNumber];
            if (currentVehicle.Engine is ElectricEngine)
            {
                throw new ArgumentException(string.Format("Can not fuel electric engine, please use charging."));
            }

            Console.WriteLine(string.Format(@"Please enter one of the following numbers for desired fuel:
1 - Soler.
2 - Octane95.
3 - Octane96
4- Octane98."));
            string inputTypeOfFuel = Console.ReadLine();
            int userSelection;
            bool isNumeric2 = int.TryParse(inputTypeOfFuel, out userSelection);
            bool validFuelType = false;

            FuelEngine.eFuelType currentVehicleFuelType = (currentVehicle.Engine as FuelEngine).m_FuelType;
            while (!validFuelType)
            {
                if (!isNumeric2 || !Enum.IsDefined(typeof(FuelEngine.eFuelType), userSelection))
                {
                    Console.WriteLine("Invalid input, please make sure to selecet a valid number.");
                    inputTypeOfFuel = Console.ReadLine();
                    isNumeric2 = int.TryParse(inputTypeOfFuel, out userSelection);
                }
                else if (currentVehicleFuelType != ConvertToEnum<FuelEngine.eFuelType>(inputTypeOfFuel))
                {
                    Console.WriteLine("Invalid input, the fuel type that you've selected doesn't match.");
                    inputTypeOfFuel = Console.ReadLine();
                    isNumeric2 = int.TryParse(inputTypeOfFuel, out userSelection);
                }
                else
                {
                    validFuelType = true;
                }
            }

            FuelEngine.eFuelType fuelType = ConvertToEnum<FuelEngine.eFuelType>(inputTypeOfFuel);

            Console.WriteLine("What amount of fuel would you like to fill? ");
            string amountToAddInput = Console.ReadLine();
            float amountToAdd;
            bool isNumeric = float.TryParse(amountToAddInput, out amountToAdd);

            while (!isNumeric || amountToAdd <= 0)
            {
                Console.WriteLine("Invalid input, please try again. Make sure to enter a positive number.");
                amountToAddInput = Console.ReadLine();
                isNumeric = float.TryParse(amountToAddInput, out amountToAdd);
            }

            i_CurrentGarage.Refuel(licenseNumber, fuelType, amountToAdd);
        }

        public static void RechargeVehicle(Garage i_CurrentGarage)
        {
            Console.WriteLine("Please enter license number: ");
            string licenseNumber = Console.ReadLine();

            while (!i_CurrentGarage.VehiclesInGarage.ContainsKey(licenseNumber))
            {
                Console.WriteLine("License number not found, please try again.");
                licenseNumber = Console.ReadLine();
            }

            if (i_CurrentGarage.VehiclesInGarage[licenseNumber].Engine is FuelEngine)
            {
                throw new ArgumentException(string.Format("Can not electrically charge fuel engine, please use refueling."));
            }

            Console.WriteLine("How many minutes would you like to charge? ");
            string minutesToAddInput = Console.ReadLine();
            int minutesToAdd;
            bool isNumeric = int.TryParse(minutesToAddInput, out minutesToAdd);

            while (!isNumeric || minutesToAdd <= 0)
            {
                Console.WriteLine("Invalid input, please try again. Make sure to enter a positive number.");
                minutesToAddInput = Console.ReadLine();
                isNumeric = int.TryParse(minutesToAddInput, out minutesToAdd);
            }

            i_CurrentGarage.Charge(licenseNumber, minutesToAdd);
            Console.WriteLine("Battery was charged.");
        }

        public static void DisplayVehicleInfo(Garage i_CurrentGarage)
        {
            Console.WriteLine("Please enter license number: ");
            string licenseNumber = Console.ReadLine();

            while (!i_CurrentGarage.VehiclesInGarage.ContainsKey(licenseNumber))
            {
                Console.WriteLine("License number not found, please try again.");
                licenseNumber = Console.ReadLine();
            }

            Vehicle currentVehicle = i_CurrentGarage.VehiclesInGarage[licenseNumber];
            Console.WriteLine(currentVehicle.ToString());
        }

        public static T ConvertToEnum<T>(object i_Object)
        {
            T enumVal = (T)Enum.Parse(typeof(T), i_Object.ToString());

            return enumVal;
        }

        public enum eMenuOptions
        {
            InsertVehicle = 1,
            GetLicensesInGarage,
            UpdateStatus,
            InflateTiresToMax,
            Refuel,
            Charge,
            VehicleInfo,
            Exit
        }

        public static string SplitByCapitalLetters(string i_Sentence)
        {
            StringBuilder newSentence = new StringBuilder();
            StringBuilder word = new StringBuilder();

            foreach (char letter in i_Sentence)
            {
                if (letter >= 'A' && letter <= 'Z')
                {
                    newSentence.Append(word.ToString());
                    newSentence.Append(" ");
                    word.Clear();
                }

                word.Append(letter);
            }

            newSentence.Append(word.ToString());

            return newSentence.ToString();
        }

        public static string GetVehicleTypeFirstWord(Creator.eVehicle i_VehicleType)
        {
            string vehicleTypeStringRep = Enum.GetName(typeof(Creator.eVehicle), i_VehicleType);
            StringBuilder vehicleTypeFirstWord = new StringBuilder();
            bool secondCapitalLetterFound = false;
            int currentLetterIndex = 1;

            vehicleTypeFirstWord.Append(vehicleTypeStringRep[0]);
            while (!secondCapitalLetterFound)
            {
                char currentCheckedLetter = vehicleTypeStringRep[currentLetterIndex++];
                if (!char.IsUpper(currentCheckedLetter))
                {
                    vehicleTypeFirstWord.Append(currentCheckedLetter);
                }
                else
                {
                    secondCapitalLetterFound = true;
                }
            }

            return vehicleTypeFirstWord.ToString();
        }

        public static Creator.eVehicle ReceiveVehcileType()
        {
            Console.WriteLine("Please enter vehicle type.");
            PrintEnum(typeof(Creator.eVehicle));
            string vehicleTypeInput = Console.ReadLine();

            Console.Clear();
            int vehicleTypeNumber;
            bool isNumericVehicleType = int.TryParse(vehicleTypeInput, out vehicleTypeNumber);

            while (!isNumericVehicleType || !Enum.IsDefined(typeof(Creator.eVehicle), vehicleTypeNumber))
            {
                Console.WriteLine("Vehicle type doesn't exist, please try again.");
                vehicleTypeInput = Console.ReadLine();
                isNumericVehicleType = int.TryParse(vehicleTypeInput, out vehicleTypeNumber);
            }
            return ConvertToEnum<Creator.eVehicle>(vehicleTypeInput);
        }

        public static float ReceiveRemainingEnergy(Vehicle i_CurrentVehicle)
        {
            Console.WriteLine("Please enter remaining energy in vehicle.");
            string remainingEnergyInput = Console.ReadLine();
            float remainingEnergy;
            bool isNumeric = float.TryParse(remainingEnergyInput, out remainingEnergy);

            while (!isNumeric || remainingEnergy > i_CurrentVehicle.Engine.Max)
            {
                Console.WriteLine("Invalid input. Note that the engine's max energy is {0}. Please try again.", i_CurrentVehicle.Engine.Max);
                remainingEnergyInput = Console.ReadLine();
                isNumeric = float.TryParse(remainingEnergyInput, out remainingEnergy);
            }
            return remainingEnergy;
        }

        public static void ReceiveTiresAirPressure(Vehicle i_CurrentVehicle)
        {
            Console.WriteLine(string.Format(@"Please enter the current air pressure of each of the vehicle's wheels tires.
If all tires have the same air pressure, enter 1.
If you wish to enter each tire's air pressure individually, enter 2."));
            string tiresAirPressureSameOrNot = Console.ReadLine();

            while (tiresAirPressureSameOrNot != "1" && tiresAirPressureSameOrNot != "2")
            {
                Console.WriteLine("Invalid input, please try again.");
                tiresAirPressureSameOrNot = Console.ReadLine();
            }

            if (tiresAirPressureSameOrNot == "1")
            {
                Console.WriteLine("Please enter your tires common air pressure.");
                string tiresCommonAirPressureInput = Console.ReadLine();
                float tiresCommonAirPressure;
                bool isNumeric = float.TryParse(tiresCommonAirPressureInput, out tiresCommonAirPressure);

                while (!isNumeric || tiresCommonAirPressure > i_CurrentVehicle.Wheels[0].MaxAirPressure)
                {
                    Console.WriteLine("Invalid input. Note that the tire's max pressure is {0}. Please try again.", i_CurrentVehicle.Wheels[0].MaxAirPressure);
                    tiresCommonAirPressureInput = Console.ReadLine();
                    isNumeric = float.TryParse(tiresCommonAirPressureInput, out tiresCommonAirPressure);
                }

                foreach (Wheel wheel in i_CurrentVehicle.Wheels)
                {
                    wheel.CurrentAirPressure = tiresCommonAirPressure;
                }
            }
            else
            {
                int tiresCounter = 1;

                foreach (Wheel wheel in i_CurrentVehicle.Wheels)
                {
                    Console.WriteLine("Please enter tire number {0}'s air pressure.", tiresCounter++);
                    string currentTireAirPressureInput = Console.ReadLine();
                    float currentTireAirPressure;
                    bool isNumeric = float.TryParse(currentTireAirPressureInput, out currentTireAirPressure);

                    while (!isNumeric || currentTireAirPressure > i_CurrentVehicle.Wheels[0].MaxAirPressure)
                    {
                        Console.WriteLine("Invalid input. Note that the tire's max pressure is {0}. Please try again.", i_CurrentVehicle.Wheels[0].MaxAirPressure);
                        currentTireAirPressureInput = Console.ReadLine();
                        isNumeric = float.TryParse(currentTireAirPressureInput, out currentTireAirPressure);
                    }

                    wheel.CurrentAirPressure = currentTireAirPressure;
                }
            }
        }
    }
}
