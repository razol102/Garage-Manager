using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class VehicleFactory
    {
        private const int k_MaxLicenseLength = 8;
        private const int k_NumOfVehicleTypes = 5;
        private List<string> m_VehicleTypes;

        public VehicleFactory()
        {
            m_VehicleTypes = new List<string>(k_NumOfVehicleTypes)
            {
                "Fuel Motorcycle",
                "Electric Motorcycle",
                "Fuel Car",
                "Electric Car",
                "Truck"
            };
        }

        public List<string> VehicleTypes
        {
            get
            {
                return m_VehicleTypes;
            }
        }

        public int VehicleTypesAmount
        {
            get
            {
                return k_NumOfVehicleTypes;
            }
        }

        public int MaxLicenseLength
        {
            get
            {
                return k_MaxLicenseLength;
            }
        }

        public Vehicle CreateVehicle(int i_Type, string i_LicenseNumber)
        {
            Vehicle vehicle = null;
            Engine engine = null;

            switch (i_Type)
            {
                case 0:
                    engine = new FuelEngine(MotorCycle.k_MaxFuelTank, MotorCycle.k_FuelType);
                    vehicle = new MotorCycle(engine, i_LicenseNumber); 
                    break;
                case 1:
                    engine = new ElectricEngine(MotorCycle.k_MaxBatterySize);
                    vehicle = new MotorCycle(engine, i_LicenseNumber);
                    break;
                case 2:
                    engine = new FuelEngine(Car.k_MaxFuelTank, Car.k_FuelType);
                    vehicle = new Car(engine, i_LicenseNumber);
                    break;
                case 3:
                    engine = new ElectricEngine(Car.k_MaxBatterySize);
                    vehicle = new Car(engine, i_LicenseNumber);
                    break;
                case 4:
                    engine = new FuelEngine(Truck.k_MaxFuelTank, Truck.k_eFuelType);
                    vehicle = new Truck(engine, i_LicenseNumber);
                    break;
                default:
                    throw new ArgumentException("Invalid value!");
            }
            return vehicle;
        }
    }
}
