using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    public class ConsoleUI
    {
        private Garage m_Garage;
        private VehicleFactory m_Vehicles;
        private const int k_FirstMenuChoice = 1;
        private const int k_LastMenuChoice = 7;
        private const int k_InvalidChoice = -1;
        private const int k_PhoneNumberLength = 10;

        public ConsoleUI()
        {
            m_Garage = new Garage();
            m_Vehicles = new VehicleFactory();
        }

        public void RunGarage()
        {
            bool    endProgram = false;
            int     userMenuChoice = 0;

            printWelcomeMessage();
            while (!endProgram)
            {
                printGarageMenu();
                userMenuChoice = getMenuChoiceFromUser();
                Console.Clear();
                switch (userMenuChoice)
                {
                    case (short)eGarageMenu.AddVehicle:
                        addNewVehicle();
                        break;
                    case (short)eGarageMenu.DisplayGarageVehicles:
                        displayVehiclesByGarageStatus();
                        break;
                    case (short)eGarageMenu.ChangeVehicleStatus:
                        changeVehicleStatus();
                        break;
                    case (short)eGarageMenu.AddAirToWheels:
                        addAirToWheels();
                        break;
                    case (short)eGarageMenu.AddFuel:
                        addFuelToVehicle();
                        break;
                    case (short)eGarageMenu.ChargeBattery:
                        chargeElectricVehicle();
                        break;
                    case (short)eGarageMenu.DisplayVehicleData:
                        displayVehicleData();
                        break;
                    case (short)eGarageMenu.Exit:
                        Console.WriteLine("GoodBye!");
                        endProgram = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Enter a valid option.");
                        break;
                }

                if (!endProgram)
                {
                    printBackToMainMenu();
                }
            }
        }

        private void printWelcomeMessage()
        {
            Console.WriteLine("*******************************");
            Console.WriteLine("*                             *");
            Console.WriteLine("*    Welcome To MTA Garage!   *");
            Console.WriteLine("*                             *");
            Console.WriteLine("*******************************");
            Console.WriteLine();
        }

        private void printGarageMenu()
        {
            Console.WriteLine($@"Choose from the following options:
{(int)eGarageMenu.AddVehicle}. Add vehicle
{(int)eGarageMenu.DisplayGarageVehicles}. Display vehicles in garage
{(int)eGarageMenu.ChangeVehicleStatus}. Change vehicle status
{(int)eGarageMenu.AddAirToWheels}. Add air to wheels
{(int)eGarageMenu.AddFuel}. Add fuel
{(int)eGarageMenu.ChargeBattery}. Charge battery
{(int)eGarageMenu.DisplayVehicleData}. Display vehicle data

{(int)eGarageMenu.Exit}. Exit");
        }

        private short getMenuChoiceFromUser()
        {
            short   userChoice;
            string  userChoiceStr = Console.ReadLine();

            while (!short.TryParse(userChoiceStr, out userChoice))
            {
                Console.Write("Invalid value! Enter an integer between {0} and {1}: ", k_FirstMenuChoice - 1, k_LastMenuChoice);
                userChoiceStr = Console.ReadLine();
            }

            Console.WriteLine();

            return userChoice;
        }

        //================ 1: ADD NEW VEHICLE ================\\

        private void addNewVehicle()
        {
            Console.WriteLine("Add Vehicle:");
            Console.WriteLine("============\n");
            Customer customer = null;

            try
            {
                string licenseNumber = getValidLicenseNumberFromUser();

                customer = m_Garage.GetCustomerByLicenseNumber(licenseNumber);
                if (customer == null)
                {
                    getCustomerDataFromUser(licenseNumber);
                }
                else
                {
                    Console.WriteLine("Vehicle already in Garage. Status changed to In-Repair");
                    m_Garage.ChangeVehicleStatus(licenseNumber, eStatusInGarage.InRepair);
                }
            }
            catch (Exception i_Exception)
            {
                Console.WriteLine(i_Exception.Message);
            }
        }

        private string getValidLicenseNumberFromUser()
        {
            string  userInput;
            bool    isValidLicenseNumber = false;
            
            do
            {
                Console.Write("Enter license number: ");
                userInput = Console.ReadLine();
                if (long.TryParse(userInput, out long licenseNumber) && !string.IsNullOrEmpty(userInput))
                {
                    if (userInput.Length > m_Vehicles.MaxLicenseLength)
                    {
                        Console.WriteLine("License number must be up to {0} digits!\n", m_Vehicles.MaxLicenseLength);
                    }
                    else
                    {
                        isValidLicenseNumber = true;
                    }
                }
                else
                {
                    Console.WriteLine("License number must not be empty and contain only digits!\n");
                }
            } while (!isValidLicenseNumber);

            return userInput;
        }

        private void getCustomerDataFromUser(string i_LicenseNumber)
        {
            bool        isValidVehicleData = false;
            int         attributeEnumIndex = 0;
            Customer    customer = null;
            
            getCustomerDetails(out string customerName, out string phoneNumber);
            Vehicle vehicle = getVehicleFromUser(i_LicenseNumber);

            Console.WriteLine("\nEnter Data for the next Attributes:");
            Console.WriteLine("===================================\n");
            while (!isValidVehicleData)
            {
                try
                {
                    List<string> generalAttributesInput = getCustomerVehicleAttributes(vehicle.GeneralAttributesNames, vehicle, ref attributeEnumIndex);
                    List<string> uniqueAttributesInput = getCustomerVehicleAttributes(vehicle.GetUniqueAttributesNamesToChoose(), vehicle, ref attributeEnumIndex);

                    vehicle.SetNewVehicleAttributes(i_LicenseNumber, generalAttributesInput, uniqueAttributesInput);
                    customer = new Customer(vehicle, customerName, phoneNumber);
                    isValidVehicleData = true;
                }
                catch (Exception i_Exception)
                {
                    Console.WriteLine(i_Exception.Message);
                }
            }

            m_Garage.AddNewVehicle(customer);
            Console.WriteLine("\n{0}'s car was added successfully!", customer.Name);
        }

        private void getCustomerDetails(out string o_Name, out string o_PhoneNumber)
        {
            o_Name = getValidCustomerName();
            o_PhoneNumber = getValidCustomerPhoneNumber();
        }

        private string getValidCustomerName()
        {
            string customerName;

            Console.Write("Enter customer name: ");
            customerName = Console.ReadLine();
            while (customerName.Length < 2 || !isNameContainsOnlyLetters(customerName))
            {
                Console.WriteLine("Name should be with at least 2 chars and contain only letters.\n");
                Console.Write("Enter customer name: ");
                customerName = Console.ReadLine();
                Console.WriteLine();
            }

            customerName = customerName.First().ToString().ToUpper() + customerName.Substring(1).ToLower();

            return customerName;
        }

        private bool isNameContainsOnlyLetters(string i_NameInput)
        {
            bool isNameFullyLetters = true;

            foreach (char ch in i_NameInput)
            {
                if (!char.IsLetter(ch) && ch != '-' && ch != ' ')
                {
                    isNameFullyLetters = false;
                    break;
                }
            }

            return isNameFullyLetters;
        }

        private string getValidCustomerPhoneNumber()
        {
            string phoneNumberStr;

            Console.Write("Enter phone number ({0} digits): ", k_PhoneNumberLength);
            phoneNumberStr = Console.ReadLine();
            while (string.IsNullOrEmpty(phoneNumberStr) || phoneNumberStr.Length != k_PhoneNumberLength ||
                (!int.TryParse(phoneNumberStr, out int phoneNumber)))
            {
                Console.WriteLine("Invalid phone number entered!\nNumber should be with exactly {0} digits.\n", k_PhoneNumberLength);
                Console.Write("Enter phone number ({0} digits): ", k_PhoneNumberLength);
                phoneNumberStr = Console.ReadLine();
                Console.WriteLine();
            }

            return phoneNumberStr;
        }

        private Vehicle getVehicleFromUser(string i_LicenseNumber)
        {
            int selectedVehicleType = getVehicleTypeChoiceFromUser();

            return m_Vehicles.CreateVehicle(selectedVehicleType, i_LicenseNumber);
        }

        private int getVehicleTypeChoiceFromUser()
        {
            bool    isValidChoice = false;
            int     selectedIndex = k_InvalidChoice;
            string  typeChoice;

            while (!isValidChoice)
            {
                Console.WriteLine("Enter vehicle type ({0} - {1}):", k_FirstMenuChoice, m_Vehicles.VehicleTypesAmount);
                printVehiclesTypes();
                typeChoice = Console.ReadLine();
                isValidChoice = isValidVehicleTypeChoise(typeChoice, ref selectedIndex);
            }

            return selectedIndex - 1;
        }

        private void printVehiclesTypes()
        {
            for (int i = 0; i < m_Vehicles.VehicleTypesAmount; i++)
            {
                Console.WriteLine($"{i + 1}. {m_Vehicles.VehicleTypes[i]}");
            }
        }

        private bool isValidVehicleTypeChoise(string i_Choice, ref int io_SelectedIndex)
        {
            bool isValidChoice = false;

            if (!int.TryParse(i_Choice, out io_SelectedIndex))
            {
                Console.WriteLine("Invalid input. Try again.\n");
            }
            else if (io_SelectedIndex < k_FirstMenuChoice || io_SelectedIndex > m_Vehicles.VehicleTypesAmount)
            {
                Console.WriteLine("Invalid vehicle type choice.\n");
            }
            else
            {
                isValidChoice = true;
            }

            return isValidChoice;
        }

        private List<string> getCustomerVehicleAttributes(List<string> i_Attributes, Vehicle i_CurrentVehicle, ref int io_AttributeEnumIndex)
        {
            List<string>    dataInputFromUser = new List<string>(i_Attributes.Count);
            string          userInput = null;
            bool            isValidAttributeInput = false;
            Type            typeOfEngine = i_CurrentVehicle.Engine.GetType();

            for (int i = 0; i < i_Attributes.Count; i++)
            {
                while (!isValidAttributeInput)
                {
                    try
                    {
                        Console.Write(i_Attributes[i] + ": ");
                        userInput = Console.ReadLine();
                        i_CurrentVehicle.CheckGeneralAttributesInput(io_AttributeEnumIndex, userInput, typeOfEngine);
                        isValidAttributeInput = true;
                    }
                    catch (Exception i_Exception)
                    {
                        Console.WriteLine(i_Exception.Message);
                        Console.WriteLine();
                    }
                }

                isValidAttributeInput = false;
                io_AttributeEnumIndex++;
                dataInputFromUser.Add(userInput);
            }

            return dataInputFromUser;
        }

        //================ 2: DISPLAY VEHICLE BY GARAGE STATUS ================\\

        private void displayVehiclesByGarageStatus()
        {
            Console.WriteLine("Show vehicles by status in garage:");
            Console.WriteLine("========================\n");
            displayVehiclesByStatus(getValidVehicleStatus());
        }

        private eStatusInGarage getValidVehicleStatus()
        {
            int             userInput;
            string          statusChoice;
            bool            isValidInput = false;
            eStatusInGarage status = 0;

            do
            {
                printGarageStatuses();
                statusChoice = Console.ReadLine();
                if (int.TryParse(statusChoice, out userInput) && !string.IsNullOrEmpty(statusChoice))
                {
                    if (0 <= userInput - 1 && userInput <= Enum.GetValues(typeof(eStatusInGarage)).Length)
                    {
                        isValidInput = true;
                        status = (eStatusInGarage)userInput;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Enter a number between 1 to {0}\n", Enum.GetValues(typeof(eStatusInGarage)).Length);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Enter a valid number\n");
                }
            } while (!isValidInput);

            return status - 1;
        }

        private void printGarageStatuses()
        {
            int             indexOfStatus = 1;
            StringBuilder   statusSB = new StringBuilder();

            foreach (eStatusInGarage status in Enum.GetValues(typeof(eStatusInGarage)))
            {
                statusSB.AppendFormat("{0} - {1}, ", indexOfStatus, status);
                indexOfStatus++;
            }

            statusSB.Length -= 2;
            statusSB.AppendFormat("\nEnter the wanted status number (1 - {0}): ", Enum.GetValues(typeof(eStatusInGarage)).Length);
            Console.Write(statusSB);
        }

        private void printBackToMainMenu()
        {
            Console.Write("\nPress any key to go back to main menu...");
            Console.ReadKey();
            Console.Clear();
        }

        private void displayVehiclesByStatus(eStatusInGarage i_Status)
        {
            List<string>    licenseNumbers = m_Garage.GetLicenseNumbersByStatus(i_Status.ToString());
            int             licenseNumberIndex = 1;

            Console.WriteLine($"\nVehicles with status '{i_Status}':");
            if (licenseNumbers.Count == 0)
            {
                Console.WriteLine("There are no vehicles with the specified status found");
            }
            else
            {
                foreach (string licenseNumber in licenseNumbers)
                {
                    Console.WriteLine($"{licenseNumberIndex++} - {licenseNumber}");
                }
            }
        }

        //================ 3: CHANGE VEHICLE STATUS ================\\

        private void changeVehicleStatus()
        {
            Console.WriteLine("Change vehicle status:");
            Console.WriteLine("======================\n");
            string licenseNumber = getValidLicenseNumberFromUser();
            
            if (m_Garage.IsGarageEmpty())
            {
                Console.WriteLine(m_Garage.EmptyGarageMessege);
            }
            else if (m_Garage.GetCustomerByLicenseNumber(licenseNumber) == null)
            {
                Console.WriteLine("Vehicle is not found in the garage!");
            }
            else
            {
                Console.Write("Avaiable statuses: ");
                eStatusInGarage newVehicleStatus = getValidVehicleStatus();

                m_Garage.ChangeVehicleStatus(licenseNumber, newVehicleStatus);
                Console.WriteLine("\nVehicle with license number: {0} status has changed to '{1}' successfully", licenseNumber, newVehicleStatus);
            }
        }

        //================ 4: ADD AIR TO WHEELS ================\\

        private void addAirToWheels()
        {
            Console.WriteLine("Add air to wheels:");
            Console.WriteLine("==================\n");
            string licenseNumber = getValidLicenseNumberFromUser();

            if (m_Garage.IsGarageEmpty())
            {
                Console.WriteLine(m_Garage.EmptyGarageMessege);
            }
            else if (m_Garage.GetCustomerByLicenseNumber(licenseNumber) == null)
            {
                Console.WriteLine("Vehicle is not found in the garage!");
            }
            else
            {
                m_Garage.InflateAllWheelsToMax(licenseNumber);
                Console.WriteLine("Inflating vehicle with license number: {0} wheels air pressure to maximum capacity...", licenseNumber);
            }
        }

        //================ 5: ADD FUEL TO VEHICLE ================\\

        private void addFuelToVehicle()
        {
            Console.WriteLine("Add fuel to vehicle:");
            Console.WriteLine("====================\n");
            bool isValidInput = false;

            if (m_Garage.IsGarageEmpty())
            {
                Console.WriteLine(m_Garage.EmptyGarageMessege);
            }
            else
            {
                string licenseNumber = getValidLicenseNumberFromUser();

                if (m_Garage.GetCustomerByLicenseNumber(licenseNumber) == null)
                {
                    Console.WriteLine("Vehicle is not found in the garage!");
                }
                else if (m_Garage.GarageData[licenseNumber].Vehicle.Engine is ElectricEngine)
                {
                    Console.WriteLine("Cannot add fuel to an electric engine!");
                }
                else
                {
                    while (!isValidInput)
                    {
                        try
                        {
                            eFuelType   fuelType = getValidFuelType();
                            float       fuelAmountToAdd = getEnergyAmountToAddFromUser("liters");

                            isValidInput = true;
                            m_Garage.AddEnergy(licenseNumber, fuelType, fuelAmountToAdd);
                            Console.WriteLine("\nVehicle with license number: {0} fuel amount set to {1} successfully", licenseNumber, m_Garage.GarageData[licenseNumber].Vehicle.Engine.CurrentEnergyLevel);
                        }
                        catch (ValueOutOfRangeException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("General error occured!");
                        }
                    }
                }
            }
        }

        private float getEnergyAmountToAddFromUser(string i_Unit)
        {
            string energyAmountStr = null;

            Console.Write("Enter wanted amount to add in {0}: ", i_Unit);
            energyAmountStr = Console.ReadLine();
            if (!float.TryParse(energyAmountStr, out float energyAmount) || string.IsNullOrEmpty(energyAmountStr))
            {
                throw new FormatException("Invalid choice. Try again");
            }

            return energyAmount;
        }

        private eFuelType getValidFuelType()
        {
            int             indexOfFuelType = 1;
            StringBuilder   fuelSB = new StringBuilder();
            int             eFuleTypeAmount = Enum.GetValues(typeof(eFuelType)).Length;
            string          fuelChoiceStr = null;

            foreach (eFuelType fuel in Enum.GetValues(typeof(eFuelType)))
            {
                fuelSB.AppendFormat("{0} - {1}, ", indexOfFuelType, fuel);
                indexOfFuelType++;
            }

            fuelSB.Length -= 2;
            fuelSB.AppendFormat("\nEnter the wanted fuel type number (1 - {0}): ", eFuleTypeAmount);
            Console.Write("Avaiable fuel types: ");
            Console.Write(fuelSB);
            fuelChoiceStr = Console.ReadLine();

            if (int.TryParse(fuelChoiceStr, out int fuelChoice) && !string.IsNullOrEmpty(fuelChoiceStr))
            {
                if (0 >= fuelChoice || fuelChoice > eFuleTypeAmount)
                {
                    throw new ValueOutOfRangeException(eFuleTypeAmount, 1, $"Invalid choice. Enter a number between 1 to {eFuleTypeAmount}\n");
                }
            }
            else
            {
                throw new FormatException("Invalid choice. Try again");
            }

            return (eFuelType)fuelChoice - 1;
        }

        //================ 6: CHARGE ELECTRIC VEHICLE ================\\

        private void chargeElectricVehicle()
        {
            bool isValidInput = false;

            Console.WriteLine("Charge electric vehicle:");
            Console.WriteLine("=======================\n");
            if (m_Garage.IsGarageEmpty())
            {
                Console.WriteLine(m_Garage.EmptyGarageMessege);
            }
            else
            {
                string licenseNumber = getValidLicenseNumberFromUser();
                
                if (m_Garage.GetCustomerByLicenseNumber(licenseNumber) == null)
                {
                    Console.WriteLine("Vehicle is not found in the garage!");
                }
                else if (m_Garage.GarageData[licenseNumber].Vehicle.Engine is FuelEngine)
                {
                    Console.WriteLine("Cannot add battery to a fuel engine!");
                }
                else
                {
                    while (!isValidInput)
                    {
                        try
                        {
                            float minutesToAddToBattery = getEnergyAmountToAddFromUser("minutes");

                            isValidInput = true;
                            m_Garage.AddEnergy(licenseNumber, null, minutesToAddToBattery);
                            Console.WriteLine("\nVehicle with license number: {0} battery level set to {1} successfully", licenseNumber, m_Garage.GarageData[licenseNumber].Vehicle.Engine.CurrentEnergyLevel);
                        }
                        catch (ValueOutOfRangeException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("General error occured!");
                        }
                    }
                }
            }
        }

        //================ 7: DISPLAY VEHICLE DATA ================\\

        private void displayVehicleData()
        {
            Console.WriteLine("Display vehicle data:");
            Console.WriteLine("=====================\n");
            if (m_Garage.IsGarageEmpty())
            {
                Console.WriteLine(m_Garage.EmptyGarageMessege);
            }
            else
            {
                string      licenseNumber = getValidLicenseNumberFromUser();
                Customer    customer = (m_Garage.GetCustomerByLicenseNumber(licenseNumber));
                
                if (customer == null)
                {
                    Console.WriteLine("Vehicle is not found in the garage!");
                }
                else
                {
                    printChosenVehicleData(customer);
                }
            }
        }
        
        private void printChosenVehicleData(Customer i_Customer)
        {
            List<string> uniqueAttributesNames = i_Customer.Vehicle.UniqueAttributesNames;
            List<string> uniqueAttributesValues = i_Customer.Vehicle.GetUniqueAttributesValues();
            float energyLevelPrecentage = i_Customer.Vehicle.EnergyPrecentageLevel * 100;
            string energyLevelPrecentageStr = energyLevelPrecentage == (int)energyLevelPrecentage ? $"{energyLevelPrecentage:0}" : $"{energyLevelPrecentage:0.00}";

            Console.WriteLine("\nLicense number: {0}", i_Customer.Vehicle.LicenseNumber);
            Console.WriteLine("Vehicle model: {0}", i_Customer.Vehicle.Model);
            Console.WriteLine("Owner name: {0}", i_Customer.Name);
            Console.WriteLine("Vehicle status: {0}", i_Customer.Vehicle.Status);
            Console.WriteLine("Wheels: Air pressure - {0} | Manufacture - {1}", i_Customer.Vehicle.Wheels[0].CurrentAirPressure, i_Customer.Vehicle.Wheels[0].ManufacturerName);
            Console.WriteLine("Energy: Type - {0} | Level - {1}%", i_Customer.Vehicle.Engine.GetEnergyType(), energyLevelPrecentageStr);
            Console.WriteLine("{0}: {1}", uniqueAttributesNames[0], uniqueAttributesValues[0]);
            Console.WriteLine("{0}: {1}", uniqueAttributesNames[1], uniqueAttributesValues[1]);
        }
    }
}
