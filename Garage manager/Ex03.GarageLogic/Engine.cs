using System;

namespace Ex03.GarageLogic
{
    public abstract class Engine
    {
        protected float m_CurrentEnergyLevel;
        protected readonly float r_MaxEnergyLevelCapacity;

        protected Engine(float i_MaxCapacity)
        {
            if (i_MaxCapacity > 0)
            {
                r_MaxEnergyLevelCapacity = i_MaxCapacity;
            }
            else
            {
                throw new ValueOutOfRangeException(null, 0, "Engine capacity must be greater than 0!");
            }

            m_CurrentEnergyLevel = 0;
        }

        public float CurrentEnergyLevel
        {
            get
            {
                return m_CurrentEnergyLevel;
            }

            internal set
            {
                if (m_CurrentEnergyLevel == r_MaxEnergyLevelCapacity)
                {
                    throw new FormatException("Energy level is already full.");
                }
                else if (value >= 0 && value <= r_MaxEnergyLevelCapacity)
                {
                    m_CurrentEnergyLevel = value;
                }
                else
                {
                    string exMessage = $"Energy level must be between 0 to {r_MaxEnergyLevelCapacity}. You can add up to {r_MaxEnergyLevelCapacity - m_CurrentEnergyLevel}.";
                    throw new ValueOutOfRangeException(r_MaxEnergyLevelCapacity, 0, exMessage);
                }
            }
        }

        public float MaxCapacity
        {
            get
            {
                return r_MaxEnergyLevelCapacity;
            }
        }

        public abstract void AddEnergy(float i_EnergyToAdd, eFuelType? i_FuelTypeToAdd);

        public abstract string GetEnergyType();
    }
}