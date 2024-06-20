using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class Truck : Vehicle
    {
        private bool m_IsCarryingDangerousMaterials;
        private float m_CargoVolume;
        internal const float k_MaxFuelTank = 120f;
        internal const eFuelType k_eFuelType = eFuelType.Soler;
        private const int k_NumOfWheels = 12;
        private const float k_MaxWheelAirPressure = 28f;
        private List<string> m_UniqueAttributesNames;

        public Truck(Engine i_Engine, string i_LicenseNumber) : base(i_Engine, i_LicenseNumber)
        {
            m_IsCarryingDangerousMaterials = false;
            m_CargoVolume = 0;
            m_Wheels = CreateWheelsList(k_NumOfWheels, k_MaxWheelAirPressure);
            m_UniqueAttributesNames = new List<string>(k_NumOfUniqueAttributes)
            {
                "Containing Dangerous Materials",
                "Cargo Volume",
            };
        }

        public override List<string> UniqueAttributesNames
        {
            get
            {
                return m_UniqueAttributesNames;
            }
        }

        public override List<string> GetUniqueAttributesNamesToChoose()
        {
            return new List<string> { "Is Containing Dangerous Materials (yes\\no)", "Cargo Volume" };
        }

        internal override void SetUniqueAttributes(List<string> i_Attributes)
        {
            int indexOfAttribute1 = (int)eGeneralVehicleAttributes.Unique1 - k_NumOfGeneralAttributes;
            int indexOfAttribute2 = (int)eGeneralVehicleAttributes.Unique2 - k_NumOfGeneralAttributes;

            m_IsCarryingDangerousMaterials = (i_Attributes[indexOfAttribute1].ToLower() == "yes");
            m_CargoVolume = float.Parse(i_Attributes[indexOfAttribute2]);
        }

        internal override float GetMaxFuelAmount()
        {
            return k_MaxFuelTank;
        }

        internal override float GetMaxBatteryAmount()
        {
            return -1f;
        }

        internal override float GetMaxWheelAirPressure()
        {
            return k_MaxWheelAirPressure;
        }

        internal override void ValidationOfUnique1Attribute(string i_UserInput)
        {
            bool isCarryingDangerousValidAnswer = i_UserInput.ToLower() == "yes" || i_UserInput.ToLower() == "no";

            if(!isCarryingDangerousValidAnswer)
            {
                throw new FormatException("Answer must be 'yes' or 'no'");
            }
        }

        internal override void ValidationOfUnique2Attribute(string i_UserInput)
        {
            if (!float.TryParse(i_UserInput, out float cargoVolume))
            {
                throw new FormatException("Truck cargo volume should contain only numbers");
            }

            if (cargoVolume < 0)
            {
                throw new ValueOutOfRangeException(default, 0, "Truck cargo volume must be greater or equal to 0!");
            }
        }

        public override List<string> GetUniqueAttributesValues()
        {
            List<string> uniqueAttributes = new List<string>(k_NumOfUniqueAttributes);

            uniqueAttributes.Add(m_IsCarryingDangerousMaterials ? "Yes" : "No");
            uniqueAttributes.Add(m_CargoVolume.ToString());

            return uniqueAttributes;
        }
    }
}
