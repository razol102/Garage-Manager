using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class Garage
    {
        private Dictionary<string, Customer> m_GarageData;
        private const string k_EmptyGarageMessage = "There are no vehicles in the garage!";

        public Garage()
        {
            m_GarageData = new Dictionary<string, Customer>();
        }

        public Dictionary<string, Customer> GarageData
        {
            get
            {
                return m_GarageData;
            }
        }

        public string EmptyGarageMessege
        {
            get
            {
                return k_EmptyGarageMessage;
            }
        }

        public Customer GetCustomerByLicenseNumber(string i_LicenseNumber)
        {
            Customer customer = null;

            if (m_GarageData.ContainsKey(i_LicenseNumber))
            {
                customer = m_GarageData[i_LicenseNumber];
            }

            return customer;
        }

        public void AddNewVehicle(Customer i_Customer)
        {
            m_GarageData.Add(i_Customer.Vehicle.LicenseNumber, i_Customer);
        }

        public void ChangeVehicleStatus(string i_LicenseNumber, eStatusInGarage i_Status)
        {
            if (m_GarageData.ContainsKey(i_LicenseNumber))
            {
                m_GarageData[i_LicenseNumber].Vehicle.Status = i_Status;
            }
            else
            {
                throw new ArgumentException("Vehicle is not found!");
            }
        }

        public List<string> GetLicenseNumbersByStatus(string i_Status)
        {
            List<string> licenseNumbers = new List<string>();

            foreach (Customer customer in m_GarageData.Values)
            {
                if (customer.Vehicle.Status.ToString().Equals(i_Status))
                {
                    licenseNumbers.Add(customer.Vehicle.LicenseNumber);
                }
            }

            return licenseNumbers;
        }

        public bool IsGarageEmpty()
        {
            return m_GarageData.Count == 0;
        }

        public void InflateAllWheelsToMax(string i_LicenseNumber)
        {
            if (m_GarageData.ContainsKey(i_LicenseNumber))
            {
                m_GarageData[i_LicenseNumber].Vehicle.InflateAllWheels();
            }
            else
            {
                throw new ArgumentException("Vehicle is not found in the garage!");
            }
        }

        public void AddEnergy(string i_LicenseNumber, eFuelType? i_FuelType, float i_Amount)
        {
            if (m_GarageData.ContainsKey(i_LicenseNumber))
            {
                m_GarageData[i_LicenseNumber].Vehicle.AddEnergyToVehicle(i_Amount, i_FuelType);
            }
            else
            {
                throw new ArgumentException("Vehicle is not found in the garage!");
            }
        }
    }
}
