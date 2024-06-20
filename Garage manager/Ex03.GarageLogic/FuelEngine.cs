using System;

namespace Ex03.GarageLogic
{
    public class FuelEngine : Engine
    {
        private readonly eFuelType r_FuelType;

        internal FuelEngine(float i_MaxEngineTank, eFuelType i_FuelType) : base(i_MaxEngineTank)
        {
            r_FuelType = i_FuelType;
        }

        public eFuelType FuelType
        {
            get
            {
                return r_FuelType;
            }
        }

        public override void AddEnergy(float i_EnergyAmountToAdd, eFuelType? i_FuelTypeToAdd)
        {
            if (r_FuelType == i_FuelTypeToAdd)
            {
                CurrentEnergyLevel += i_EnergyAmountToAdd;
            }
            else
            {
                throw new FormatException($"Wrong Fuel Type! Fuel Type must be {r_FuelType.ToString()}!");
            }
        }

        public override string GetEnergyType()
        {
            return r_FuelType.ToString();
        }
    }
}
