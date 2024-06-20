using System;

namespace Ex03.GarageLogic
{
    public class Wheel
    {
        private string m_ManufacturerName;
        private float m_CurrentAirPressure;
        private readonly float r_MaxAirPressure;

        internal Wheel(float i_MaxAirPressure)
        {
            if (i_MaxAirPressure > 0)
            {
                r_MaxAirPressure = i_MaxAirPressure;
            }
            else
            {
                throw new ValueOutOfRangeException(null, 0, "Max air pressure must be greater than 0!");
            }

            m_ManufacturerName = null;
            m_CurrentAirPressure = 0;
        }

        public float CurrentAirPressure
        {
            get
            {
                return m_CurrentAirPressure;
            }

            set
            {
                if (value >= 0 && value <= r_MaxAirPressure)
                {
                    m_CurrentAirPressure = value;
                }
                else
                {
                    string exMessage = $"Air pressure must be between 0 to {r_MaxAirPressure}. You can add up to {r_MaxAirPressure - m_CurrentAirPressure}.";

                    throw new ValueOutOfRangeException(r_MaxAirPressure, 0, exMessage);
                }
            }
        }

        internal float MaxAirPressure
        {
            get
            {
                return r_MaxAirPressure;
            }
        }

        public string ManufacturerName
        {
            get
            {
                return m_ManufacturerName;
            }

            set
            {
                m_ManufacturerName = value;
            }
        }

        internal void PressureToAddToWheel(float i_AirToAdd)
        {
            float updatedAirPressure = i_AirToAdd + m_CurrentAirPressure;

            if (updatedAirPressure <= r_MaxAirPressure && updatedAirPressure >= 0)
            {
                m_CurrentAirPressure = updatedAirPressure;
            }
            else
            {
                throw new ValueOutOfRangeException(r_MaxAirPressure, 0);
            }
        }

        internal void SetAttributes(string i_ManufacturerName, string i_CurrentAirPressure)
        {
            m_ManufacturerName = i_ManufacturerName;
            m_CurrentAirPressure = float.Parse(i_CurrentAirPressure);
        }
    }
}
