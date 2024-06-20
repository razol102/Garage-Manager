using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public abstract class Vehicle
    {
        protected const int k_NumOfGeneralAttributes = 4;
        protected const int k_NumOfUniqueAttributes = 2;
        protected string m_Model;
        protected string m_LicenseNumber;
        protected float m_EnergyPrecentageLevel;
        protected List<Wheel> m_Wheels;
        protected Engine m_Engine;
        private eStatusInGarage m_Status;
        private List<string> m_GeneralAttributesNames;

        public Vehicle(Engine i_Engine, string i_LicenseNumber)
        {
            m_Engine = i_Engine;
            m_Model = null;
            m_LicenseNumber = i_LicenseNumber;
            m_EnergyPrecentageLevel = 0;
            m_Wheels = null;
            m_Status = eStatusInGarage.InRepair;
            m_GeneralAttributesNames = new List<string>(k_NumOfGeneralAttributes)
            {
                "Current energy amount",
                "Wheels manufactorer",
                "Current wheels air pressure",
                "Vehicle model"
            };
        }

        public List<string> GeneralAttributesNames
        {
            get
            {
                return m_GeneralAttributesNames;
            }
        }

        public abstract List<string> UniqueAttributesNames { get; }

        public eStatusInGarage Status
        {
            get
            {
                return m_Status;
            }

            internal set
            {
                m_Status = value;
            }
        }

        public string LicenseNumber
        {
            get
            {
                return m_LicenseNumber;
            }

            private set
            {
                m_LicenseNumber = value;
            }
        }

        public float EnergyPrecentageLevel
        {
            get
            {
                return m_EnergyPrecentageLevel;
            }
        }

        public List<Wheel> Wheels
        {
            get
            {
                return m_Wheels;
            }
        }

        public string Model
        {
            get
            {
                return m_Model;
            }

            private set
            {
                m_Model = value;
            }
        }

        public Engine Engine
        {
            get
            {
                return m_Engine;
            }
        }

        public abstract List<string> GetUniqueAttributesNamesToChoose();

        public void SetNewVehicleAttributes(string i_LicenseNumber, List<string> i_GeneralAttributes, List<string> i_UniqueAttributes)
        {
            SetGeneralAttributes(i_GeneralAttributes);
            SetUniqueAttributes(i_UniqueAttributes);
        }

        internal void SetGeneralAttributes(List<string> i_Attributes)
        {
            m_Engine.CurrentEnergyLevel = float.Parse(i_Attributes[(int)eGeneralVehicleAttributes.Current_Energy_Amount]);
            updateEnergyPercentage();
            setAllWheelsAttributes(i_Attributes[(int)eGeneralVehicleAttributes.Wheels_Manufactorer], i_Attributes[(int)eGeneralVehicleAttributes.Current_Wheels_Air_Pressure]);
            m_Model = i_Attributes[(int)eGeneralVehicleAttributes.Vehicle_Model];
        }

        internal abstract void SetUniqueAttributes(List<string> i_Features);

        private void updateEnergyPercentage()
        {
            m_EnergyPrecentageLevel = m_Engine.CurrentEnergyLevel / m_Engine.MaxCapacity;
        }

        private void setAllWheelsAttributes(string i_Manufactorer, string i_AirPressure)
        {
            foreach (Wheel wheel in m_Wheels)
            {
                wheel.SetAttributes(i_Manufactorer, i_AirPressure);
            }
        }

        protected List<Wheel> CreateWheelsList(int i_NumOfWheels, float i_MaxAirPressure)
        {
            List<Wheel> wheels = new List<Wheel>(i_NumOfWheels);

            for (int i = 0; i < i_NumOfWheels; i++)
            {
                wheels.Add(new Wheel(i_MaxAirPressure));
            }

            return wheels;
        }

        public void CheckGeneralAttributesInput(int i_AttributeIndex, string i_UserInput, Type i_TypeOfEngine)
        {
            if (string.IsNullOrEmpty(i_UserInput))
            {
                throw new FormatException("Input can not be empty");
            }

            switch (i_AttributeIndex)
            {
                case (int)eGeneralVehicleAttributes.Current_Energy_Amount:
                    if (float.TryParse(i_UserInput, out float energyAmount))
                    {
                        if (i_TypeOfEngine == typeof(FuelEngine))
                        {
                            if (energyAmount > GetMaxFuelAmount() || energyAmount < 0)
                            {
                                throw new ValueOutOfRangeException(GetMaxFuelAmount(), 0, $"Fuel amount must be between 0 to {GetMaxFuelAmount()}");
                            }
                        }
                        else
                        {
                            if (energyAmount > GetMaxBatteryAmount() || energyAmount < 0)
                            {
                                throw new ValueOutOfRangeException(GetMaxBatteryAmount(), 0, $"Battery amount must be between 0 to {GetMaxBatteryAmount()}");
                            }
                        }
                    }
                    else
                    {
                        throw new FormatException("Energy amount must contain only numbers");
                    }

                    break;
                case (int)eGeneralVehicleAttributes.Wheels_Manufactorer:
                    if (i_UserInput.Length < 2)
                    {
                        throw new ValueOutOfRangeException(default, 2, "Wheels manufactorer must include at least 2 characters");
                    }

                    break;
                case (int)eGeneralVehicleAttributes.Current_Wheels_Air_Pressure:
                    if (float.TryParse(i_UserInput, out float wheelAirPressure))
                    {
                        if (wheelAirPressure > GetMaxWheelAirPressure() || wheelAirPressure < 0)
                        {
                            throw new ValueOutOfRangeException(GetMaxWheelAirPressure(), 0, $"Wheel air pressure must be between 0 to {GetMaxWheelAirPressure()}");
                        }
                    }
                    else
                    {
                        throw new FormatException("Wheel air pressure must contain only numbers");
                    }
                    break;
                case (int)eGeneralVehicleAttributes.Vehicle_Model:
                    if (i_UserInput.Length < 2)
                    {
                        throw new ValueOutOfRangeException(default, 2, "Vehicle model must include at least 2 characters");
                    }

                    break;
                case (int)eGeneralVehicleAttributes.Unique1:
                    ValidationOfUnique1Attribute(i_UserInput);
                    break;
                case (int)eGeneralVehicleAttributes.Unique2:
                    ValidationOfUnique2Attribute(i_UserInput);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Enter a valid option.");
                    break;
            }
        }

        internal abstract float GetMaxFuelAmount();

        internal abstract float GetMaxBatteryAmount();

        internal abstract float GetMaxWheelAirPressure();

        internal abstract void ValidationOfUnique1Attribute(string i_UserInput);

        internal abstract void ValidationOfUnique2Attribute(string i_UserInput);

        internal void InflateAllWheels()
        {
            foreach (Wheel wheel in m_Wheels)
            {
                wheel.CurrentAirPressure = wheel.MaxAirPressure;
            }
        }

        internal void AddEnergyToVehicle(float i_EnergyToAdd, eFuelType? i_FuelType)
        {
            if (m_Engine is FuelEngine fuelEnergy)
            {
                fuelEnergy.AddEnergy(i_EnergyToAdd, i_FuelType);
            }
            else if (m_Engine is ElectricEngine electricEnergy)
            {
                electricEnergy.AddEnergy(i_EnergyToAdd, i_FuelType);
            }
            else
            {
                throw new FormatException("General error occured!");
            }

            updateEnergyPercentage();
        }
        
        public abstract List<string> GetUniqueAttributesValues();
    }
}
