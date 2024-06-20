using System;

namespace Ex03.GarageLogic
{
    public class Customer
    {
        private string m_Name;
        private string m_PhoneNumber;
        private Vehicle m_Vehicle;

        public Customer(Vehicle i_Vehicle, string i_CustomerName, string i_PhoneNumber)
        {
            m_Vehicle = i_Vehicle;
            m_Name = i_CustomerName;
            m_PhoneNumber = i_PhoneNumber;
        }

        public Vehicle Vehicle
        {
            get
            {
                return m_Vehicle;
            }

            internal set
            {
                if (value != null)
                {
                    m_Vehicle = value;
                }
                else
                {
                    throw new FormatException();
                }
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }

            internal set
            {
                m_Name = value;
            }
        }

        public string PhoneNumber
        {
            get
            {
                return m_PhoneNumber;
            }

            internal set
            {
                m_PhoneNumber = value;
            }
        }
    }
}
