namespace Ex03.GarageLogic
{
    public class ElectricEngine : Engine
    {
        internal ElectricEngine(float i_MaxEngineCapacity) : base(i_MaxEngineCapacity) 
        { }

        public override void AddEnergy(float i_EnergyAmountToAdd, eFuelType? i_FuelTypeToAdd)
        {
            CurrentEnergyLevel += i_EnergyAmountToAdd;
        }

        public override string GetEnergyType()
        {
            return "Electric";
        }
    }
}
